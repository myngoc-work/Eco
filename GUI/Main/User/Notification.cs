using BLL;
using BLL.User;
using DTO;
using DTO.User;
using GUI;
using GUI.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;

namespace GUI
{
    public partial class Notification : UserControl
    {
        private NotificationBLL _notificationBLL;

        private VoskVoiceRecognizer recognizer;
        private bool isListening = false;

        private class Language
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        public Notification()
        {
            InitializeComponent();
            _notificationBLL = new NotificationBLL();
            LoadNotificationData();
            GlobalEventBus.ContractDataChanged += OnContractDataChanged;
            this.Disposed += (s, e) => { GlobalEventBus.ContractDataChanged -= OnContractDataChanged; };
            ApplyLanguage(Thread.CurrentThread.CurrentUICulture.Name);

        }
        //ngon ngu
        public void ApplyLanguage(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            ResourceManager rm = new ResourceManager("GUI.Resources.Lang", typeof(HomeForm).Assembly);

            lblNotification_Title.Text = rm.GetString("lblNotification_Title");
            lblInfo.Text = rm.GetString("lblInfo");


        }

        private void LoadNotificationData(string searchText = "")
        {
            try
            {
                panelNotifications.Controls.Clear();

                var contractBll = new ContractBLL();
                var allContracts = contractBll.GetAllContracts() ?? new List<ContractDTO>();
                var now = DateTime.Now.Date;

                var overdueContracts = allContracts
                    .Where(c => c.PlanDueDate.HasValue
                                && c.PlanDueDate.Value.Date < now
                                && (c.Status == null || !c.Status.ToLower().Contains("success")))
                    .ToList();

                if (!string.IsNullOrWhiteSpace(searchText) && searchText != "đang lắng nghe...")
                {
                    string s = searchText.Trim().ToLower();
                    overdueContracts = overdueContracts.Where(c =>
                        (!string.IsNullOrEmpty(c.ContractId) && c.ContractId.ToLower().Contains(s)) ||
                        (!string.IsNullOrEmpty(c.CustomerName) && c.CustomerName.ToLower().Contains(s))
                    ).ToList();
                }

                int yPosition = 0;
                foreach (var c in overdueContracts.OrderByDescending(c => c.PlanDueDate))
                {
                    int daysLate = c.PlanDueDate.HasValue ? (now - c.PlanDueDate.Value.Date).Days : 0;
                    var card = new NotificationCard(
                        c.ContractId,
                        c.CustomerName,
                        c.PlanDueDate,
                        daysLate
                    );

                    card.Location = new Point(0, yPosition);
                    card.Width = panelNotifications.Width - 20;
                    panelNotifications.Controls.Add(card);

                    yPosition += card.Height + 20;
                }

                if (overdueContracts.Count == 0)
                {
                    var lblNoResults = new Label();
                    lblNoResults.Text = string.IsNullOrWhiteSpace(searchText) ?
                        "Không có thông báo hợp đồng quá hạn nào." :
                        $"Không tìm thấy kết quả nào cho '{searchText}'.";
                    lblNoResults.Font = new Font("Segoe UI", 14F, FontStyle.Italic);
                    lblNoResults.ForeColor = Color.Gray;
                    lblNoResults.AutoSize = true;
                    lblNoResults.Location = new Point(50, 50);
                    panelNotifications.Controls.Add(lblNoResults);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu thông báo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnContractDataChanged()
        {
            try
            {
                var overdueService = new BLL.User.OverdueNotificationService();
                overdueService.CheckAndSendOverdueNotifications();
            }
            catch { }
            LoadNotificationData();
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            SetupLanguageSelector();
            InitializeVoiceSearch();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            LoadNotificationData(searchText);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadNotificationData(txtSearch.Text);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnSearch_Click(sender, e);
            }
        }

        #region TÍCH HỢP VOICE SEARCH: Toàn bộ logic nhận dạng giọng nói được sao chép từ ucSalesContractManagement

        private void SetupLanguageSelector()
        {
            var languages = new List<Language>
            {
                new Language { Name = "Tiếng Việt", Path = "vn" },
                new Language { Name = "English", Path = "en" }
            };
            cmbLanguage.DataSource = languages;
            cmbLanguage.DisplayMember = "Name";
            cmbLanguage.ValueMember = "Path";
            cmbLanguage.SelectedIndex = 0;
        }

        private void InitializeVoiceSearch()
        {
            recognizer?.Dispose();

            try
            {
                Language selectedLanguage = (Language)cmbLanguage.SelectedItem;
                string modelFolderName = selectedLanguage.Path;
                string sourceModelPath = Path.Combine(Application.StartupPath, "Models", modelFolderName);
                EnsureVoskNativeOnPath();
                string safeModelPath = EnsureSafeModelPath(sourceModelPath, modelFolderName);

                recognizer = new VoskVoiceRecognizer(safeModelPath);
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

                btnVoiceSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo bộ nhận dạng giọng nói Vosk:\n\n" + ex.Message,
                                "Lỗi Voice Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnVoiceSearch.Enabled = false;
                recognizer = null;
            }
        }

        private void EnsureVoskNativeOnPath()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string nativeInOutput = Path.Combine(baseDir, "runtimes", "win-x64", "native");
                string candidate = Directory.Exists(nativeInOutput) ? nativeInOutput
                    : Path.GetFullPath(Path.Combine(baseDir, "..", "..", "packages", "Vosk.0.3.38", "runtimes", "win-x64", "native"));

                if (Directory.Exists(candidate))
                {
                    string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
                    if (!path.Split(';').Any(p => string.Equals(p.TrimEnd('\\'), candidate.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)))
                    {
                        Environment.SetEnvironmentVariable("PATH", candidate + ";" + path, EnvironmentVariableTarget.Process);
                    }
                }
            }
            catch { }
        }

        private string EnsureSafeModelPath(string sourceModelPath, string languageFolder)
        {
            try
            {
                if (!Directory.Exists(sourceModelPath) ||
                    !File.Exists(Path.Combine(sourceModelPath, "am", "final.mdl")))
                {
                    throw new DirectoryNotFoundException("Thiếu dữ liệu model Vosk. Hãy đảm bảo thư mục Models đã được copy đầy đủ.");
                }

                bool hasNonAscii = sourceModelPath.Any(c => c > 127);
                if (!hasNonAscii) return sourceModelPath;

                string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EcoGreen", "VoskModels", languageFolder);
                if (!Directory.Exists(appDir))
                {
                    Directory.CreateDirectory(appDir);
                    CopyDirectoryRecursive(sourceModelPath, appDir);
                }
                return appDir;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chuẩn bị model Vosk: " + ex.Message, "Voice Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void CopyDirectoryRecursive(string sourceDir, string targetDir)
        {
            foreach (string dir in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                string rel = dir.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                Directory.CreateDirectory(Path.Combine(targetDir, rel));
            }
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string rel = file.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string dest = Path.Combine(targetDir, rel);
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                File.Copy(file, dest, true);
            }
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isListening && recognizer != null)
            {
                recognizer.Stop();
                isListening = false;
                btnVoiceSearch.FillColor = Color.Transparent; // Reset màu nền
            }
            InitializeVoiceSearch();
        }

        // Thay thế sự kiện click của button voice search
        private void btnVoiceSearch_Click(object sender, EventArgs e)
        {
            if (recognizer == null || !btnVoiceSearch.Enabled) return;

            isListening = !isListening;

            if (isListening)
            {
                recognizer.Start();
                btnVoiceSearch.FillColor = Color.LightCoral; // Thay đổi màu nền để báo hiệu đang ghi âm
                txtSearch.Text = "Đang lắng nghe...";
            }
            else
            {
                recognizer.Stop();
                btnVoiceSearch.FillColor = Color.Transparent; // Trở lại màu trong suốt
                if (txtSearch.Text == "Đang lắng nghe...")
                {
                    txtSearch.Text = "";
                }
            }
        }

        private void Recognizer_SpeechRecognized(object sender, string recognizedText)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtSearch.Text = recognizedText;

                if (isListening && recognizer != null)
                {
                    recognizer.Stop();
                    isListening = false;
                    btnVoiceSearch.FillColor = Color.Transparent; // Trở lại màu trong suốt
                }
            });
        }
        #endregion
    }
}