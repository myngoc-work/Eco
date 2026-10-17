using BLL;
using BLL.User;
using DTO;
using GUI;
using GUI.Helpers;
using GUI.Main;
using GUI.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfDocument = iTextSharp.text.Document;
using PdfFont = iTextSharp.text.Font;
using PdfImage = iTextSharp.text.Image;
using PdfRectangle = iTextSharp.text.Rectangle;
using PdfParagraph = iTextSharp.text.Paragraph;
using PdfPhrase = iTextSharp.text.Phrase;
using PdfChunk = iTextSharp.text.Chunk;
using PdfElement = iTextSharp.text.Element;
using BaseColor = iTextSharp.text.BaseColor;
using PageSize = iTextSharp.text.PageSize;
using BaseFont = iTextSharp.text.pdf.BaseFont;
using PdfPCell = iTextSharp.text.pdf.PdfPCell;
using PdfPTable = iTextSharp.text.pdf.PdfPTable;
using PdfWriter = iTextSharp.text.pdf.PdfWriter;
using PdfPageEventHelper = iTextSharp.text.pdf.PdfPageEventHelper;
using PdfContentByte = iTextSharp.text.pdf.PdfContentByte;
using ColumnText = iTextSharp.text.pdf.ColumnText;
using PdfGState = iTextSharp.text.pdf.PdfGState;


namespace GUI
{
    public partial class ucSalesContractManagement : UserControl
    {
        private ContractBLL contractBLL = new ContractBLL();
        private string selectedContractId = null;
        private List<SalesContractDTO> fullContractList;

        private VoskVoiceRecognizer recognizer;
        private bool isListening = false;

        private class Language
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        private class ParameterRow
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public string Assignment { get; set; }
            public string SampleType { get; set; }
            public string LimitRange { get; set; }
            public string Result { get; set; }
            public string Note { get; set; }
        }

        private List<ParameterRow> planningRows = new List<ParameterRow>();
        private List<ParameterRow> fieldLabRows = new List<ParameterRow>();

        public ucSalesContractManagement()
        {
            InitializeComponent();
            ApplyLanguage(Thread.CurrentThread.CurrentUICulture.Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Stop và dispose recognizer trước
                try
                {
                    if (isListening && recognizer != null)
                    {
                        recognizer.Stop();
                        isListening = false;
                    }
                    recognizer?.Dispose();
                    recognizer = null;
                }
                catch { }

                // Unsubscribe event handlers trước khi dispose
                try
                {
                    if (dgvFieldLab != null && !dgvFieldLab.IsDisposed && dgvFieldLab.IsHandleCreated)
                    {
                        dgvFieldLab.CellFormatting -= dgvFieldLab_CellFormatting;
                    }
                }
                catch { }

                // Dispose components
                try
                {
                    if (components != null)
                    {
                        components.Dispose();
                        components = null;
                    }
                }
                catch { }
            }
            base.Dispose(disposing);
        }

        private void ApplyPermissions()
        {
            // Áp dụng phân quyền cho các tab - Ẩn tab nếu không có quyền truy cập
            if (!PermissionHelper.CanAccessGeneralInfoTab())
            {
                // Ẩn tab "Thông tin chung" nếu không có quyền
                if (tabControlDetails.TabPages.Contains(tabPageGeneralInfo))
                {
                    tabControlDetails.TabPages.Remove(tabPageGeneralInfo);
                }
            }

            if (!PermissionHelper.CanAccessParametersTab())
            {
                // Ẩn tab "Nền mẫu và thông số" nếu không có quyền
                if (tabControlDetails.TabPages.Contains(tabPageParameters))
                {
                    tabControlDetails.TabPages.Remove(tabPageParameters);
                }
            }

            if (!PermissionHelper.CanAccessResultsTab())
            {
                // Ẩn tab "Kết quả" nếu không có quyền
                if (tabControlDetails.TabPages.Contains(tabPageResults))
                {
                    tabControlDetails.TabPages.Remove(tabPageResults);
                }
            }

            // Vô hiệu hóa nút Save trong tab Thông tin chung nếu không phải phòng kinh doanh hoặc admin
            if (!PermissionHelper.CanAccessGeneralInfoTab())
            {
                btnSave.Enabled = false;
                btnSave.Visible = false;
            }

            // Vô hiệu hóa nút Save Planning nếu không phải phòng kế hoạch hoặc admin
            if (!PermissionHelper.CanEditLimitRange())
            {
                btnSavePlanning.Enabled = false;
                btnSavePlanning.Visible = false;
            }

            // Vô hiệu hóa nút Save Results nếu không phải phòng hiện trường/thí nghiệm hoặc admin
            if (!PermissionHelper.CanEditResults())
            {
                btnSaveResults.Enabled = false;
                btnSaveResults.Visible = false;
            }
        }
        //ngon ngu
        public void ApplyLanguage(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            ResourceManager rm = new ResourceManager("GUI.Resources.Lang", typeof(HomeForm).Assembly);

            lblMainTitle.Text = rm.GetString("lblMainTitle");
            lblContractListTitle.Text = rm.GetString("lblContractListTitle");
            lblPanelTitle.Text = rm.GetString("lblPanelTitle");
            btnSave.Text = rm.GetString("btnSave");
            lblSalesDept_CustomerName.Text = rm.GetString("SalesDept_CustomerName");
            lblSalesDept_CustomerID.Text = rm.GetString("SalesDept_CustomerID");
            lblSalesDept_Address.Text = rm.GetString("SalesDept_Address");
            lblSalesDept_RepresentativeName.Text = rm.GetString("SalesDept_RepresentativeName");
            lblSalesDept_PhoneNumber.Text = rm.GetString("SalesDept_PhoneNumber");
            lblSalesDept_ContractSignDate.Text = rm.GetString("SalesDept_ContractSignDate");
            lblSalesDept_ExpectedResultDate.Text = rm.GetString("SalesDept_ExpectedResultDate");
            lblSalesDept_RepresentativeEmail.Text = rm.GetString("SalesDept_RepresentativeEmail");
            lblResultsTitle.Text = rm.GetString("lblResultsTitle");
            lblResultsDesc.Text = rm.GetString("lblResultsDesc");
            btnPrintReport.Text = rm.GetString("btnPrintReport");
            btnSendEmail.Text = rm.GetString("btnSendEmail");
            tabPageGeneralInfo.Text = rm.GetString("tabPageGeneralInfo");
            tabPageParameters.Text = rm.GetString("tabPageParameters");
            tabPageResults.Text = rm.GetString("tabPageResults");
        }

        // private ComboBox cmbSamples; // Đã có trong Designer

        private void ucSalesContractManagement_Load(object sender, EventArgs e)
        {
            fullContractList = contractBLL.GetContractSummaryList();
            DisplayContracts(fullContractList);

            // Áp dụng phân quyền khi load - đặc biệt cho phòng kế hoạch, hiện trường, thí nghiệm
            ApplyPermissions();

            // Cấu hình cuộn cho flowLayoutPanelContracts
            flowLayoutPanelContracts.AutoScroll = true;
            flowLayoutPanelContracts.HorizontalScroll.Enabled = false;
            flowLayoutPanelContracts.VerticalScroll.Enabled = true;
            flowLayoutPanelContracts.AutoScrollMargin = new System.Drawing.Size(0, 20);
            flowLayoutPanelContracts.AutoScrollMinSize = new System.Drawing.Size(0, 2000); // Đảm bảo có đủ không gian để cuộn
            flowLayoutPanelContracts.Visible = true; // Đảm bảo FlowLayoutPanel hiển thị

            // Đảm bảo panelParametersScroll có thể scroll
            panelParametersScroll.AutoScroll = true;
            panelParametersScroll.HorizontalScroll.Enabled = false;
            panelParametersScroll.VerticalScroll.Enabled = true;

            // Thêm một control ẩn ở cuối để tạo không gian scroll
            System.Windows.Forms.Panel spacerPanel = new System.Windows.Forms.Panel();
            spacerPanel.Location = new System.Drawing.Point(0, 1500);
            spacerPanel.Size = new System.Drawing.Size(1, 1);
            spacerPanel.Visible = false;
            panelParametersScroll.Controls.Add(spacerPanel);
            InitializeParameterGrids();

            SetupLanguageSelector();

            // Thanh chọn nền mẫu cho hợp đồng
            this.cmbSamples.SelectedIndexChanged += (s, ev) =>
            {
                if (cmbSamples.SelectedItem is DTO.ContractItemDTO item)
                {
                    lblSampleTitle.Text = $"Nền mẫu: {item.ItemId} - {item.SampleName}";
                    LoadParametersForItem(item.ItemId);
                }
            };

            // Event handler cho combobox loại mẫu để lọc thông số
            if (cmbItemSampleType != null)
            {
                cmbItemSampleType.SelectedIndexChanged += CmbItemSampleType_SelectedIndexChanged;
            }

            // Không tự động đồng bộ từ ComboBox nữa - để người dùng tự chỉnh từng thông số
            // ComboBox chỉ để hiển thị/đặt giá trị mặc định ban đầu

            // Thêm validation real-time cho số điện thoại và email
            txtPhoneNumber.TextChanged += TxtPhoneNumber_TextChanged;
            txtRepresentativeEmail.TextChanged += TxtRepresentativeEmail_TextChanged;

            // Validation real-time cho tên doanh nghiệp (không chứa số)
            txtCompanyName.TextChanged += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
                {
                    txtCompanyName.FillColor = Color.White;
                }
                else if (IsValidCompanyNameNoDigits(txtCompanyName.Text))
                {
                    txtCompanyName.FillColor = Color.LightGreen;
                }
                else
                {
                    txtCompanyName.FillColor = Color.Red;
                }
            };


            // Validation real-time cho tên người đại diện (không chứa số)
            txtRepresentativeName.TextChanged += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtRepresentativeName.Text))
                {
                    txtRepresentativeName.FillColor = Color.White;
                }
                else if (IsValidCompanyNameNoDigits(txtRepresentativeName.Text))
                {
                    txtRepresentativeName.FillColor = Color.LightGreen;
                }
                else
                {
                    txtRepresentativeName.FillColor = Color.Red;
                }
            };

            // Tự động cập nhật ngày dự kiến trả kết quả = ngày ký + 20 ngày (chỉ khi người dùng thay đổi)
            dtpSigningDate.ValueChanged += DtpSigningDate_ValueChanged;

            // Kích hoạt lại voice search
            InitializeVoiceSearch();
        }

        private void tabControlDetails_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(99)))), ((int)(((byte)(63)))));
            Color inactiveColor = System.Drawing.Color.Gray;
            System.Drawing.Font activeFont = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            System.Drawing.Font inactiveFont = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);

            TabPage currentPage = this.tabControlDetails.TabPages[e.Index];


            using (SolidBrush backBrush = new SolidBrush(this.tabControlDetails.Parent.BackColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            System.Drawing.Font currentFont;
            Brush textBrush;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                currentFont = activeFont;
                textBrush = new SolidBrush(activeColor);
            }
            else
            {
                currentFont = inactiveFont;
                textBrush = new SolidBrush(inactiveColor);
            }

            TextRenderer.DrawText(e.Graphics, currentPage.Text, currentFont, e.Bounds,
                (e.State & DrawItemState.Selected) == DrawItemState.Selected ? activeColor : inactiveColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                using (Pen underlinePen = new Pen(activeColor, 3))
                {
                    e.Graphics.DrawLine(underlinePen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
                }
            }

            textBrush.Dispose();
        }


        #region Vosk Offline Voice Search Implementation

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

                // Đảm bảo PATH chứa thư mục native của Vosk (tránh lỗi load sai định dạng)
                EnsureVoskNativeOnPath();

                // Chuẩn bị đường dẫn model an toàn (ASCII) để tránh lỗi native lib với unicode path
                string safeModelPath = EnsureSafeModelPath(sourceModelPath, modelFolderName);

                recognizer = new VoskVoiceRecognizer(safeModelPath);
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

                picMicrophoneIcon.Enabled = true;
                picMicrophoneIcon.Cursor = Cursors.Hand;
            }
            catch (AccessViolationException ex)
            {
                // Xử lý lỗi AccessViolationException một cách an toàn
                MessageBox.Show("Lỗi khởi tạo bộ nhận dạng giọng nói:\n\n" +
                    "Có thể do xung đột kiến trúc x86/x64 hoặc thiếu thư viện native.\n" +
                    "Tính năng tìm kiếm bằng giọng nói sẽ bị tắt.\n\n" +
                    "Chi tiết: " + ex.Message,
                    "Lỗi Voice Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                picMicrophoneIcon.Enabled = false;
                picMicrophoneIcon.Cursor = Cursors.Default;
                recognizer = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo bộ nhận dạng giọng nói Vosk:\n\n" + ex.Message,
                                "Lỗi Voice Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                picMicrophoneIcon.Enabled = false;
                picMicrophoneIcon.Cursor = Cursors.Default;
                recognizer = null;
            }
        }

        // Cố gắng đưa thư mục native vào PATH để nạp libvosk.dll đúng kiến trúc
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
                    if (!path.Split(';').Any(p => string.Equals(p.TrimEnd('\u005c'), candidate.TrimEnd('\u005c'), StringComparison.OrdinalIgnoreCase)))
                    {
                        Environment.SetEnvironmentVariable("PATH", candidate + ";" + path, EnvironmentVariableTarget.Process);
                    }
                }
            }
            catch { }
        }

        // Nếu đường dẫn model chứa ký tự không ASCII hoặc thiếu file cốt lõi, copy ra thư mục ASCII trong LocalAppData
        private string EnsureSafeModelPath(string sourceModelPath, string languageFolder)
        {
            try
            {
                // Kiểm tra tồn tại và các file tối thiểu của Vosk
                if (!Directory.Exists(sourceModelPath) ||
                    !File.Exists(Path.Combine(sourceModelPath, "am", "final.mdl")) ||
                    !File.Exists(Path.Combine(sourceModelPath, "graph", "Gr.fst")))
                {
                    throw new DirectoryNotFoundException("Thiếu dữ liệu model Vosk. Hãy đảm bảo thư mục Models đã được copy đầy đủ.");
                }

                bool hasNonAscii = sourceModelPath.Any(c => c > 127);
                if (!hasNonAscii)
                {
                    return sourceModelPath; // đường dẫn ổn
                }

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
                picMicrophoneIcon.BackColor = Color.White;
            }
            InitializeVoiceSearch();
        }

        private void picMicrophoneIcon_Click(object sender, EventArgs e)
        {
            if (recognizer == null || !picMicrophoneIcon.Enabled) return;

            isListening = !isListening;

            if (isListening)
            {
                recognizer.Start();
                picMicrophoneIcon.BackColor = Color.LightCoral;
                txtSearch.Text = "Đang lắng nghe...";
            }
            else
            {
                recognizer.Stop();
                picMicrophoneIcon.BackColor = Color.White;
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
                    picMicrophoneIcon.BackColor = Color.White;
                }
            });
        }
        #endregion

        #region Original Methods

        private void DisplayContracts(List<SalesContractDTO> contractsToDisplay)
        {
            flowLayoutPanelContracts.Controls.Clear();
            foreach (var contract in contractsToDisplay)
            {
                ucContractItem item = new ucContractItem();
                item.SetData(contract.ContractID, contract.CompanyName, contract.ExpectedResultDate.ToString("dd/MM/yyyy"));
                item.Tag = contract.ContractID;
                item.Click += Item_Click;

                if (selectedContractId != null && contract.ContractID == selectedContractId)
                {
                    item.BackColor = Color.FromArgb(45, 100, 45);
                    // Đổi màu chữ thành trắng khi được chọn
                    foreach (Control control in item.Controls)
                    {
                        if (control is Label label)
                        {
                            label.ForeColor = Color.White;
                        }
                    }
                }

                flowLayoutPanelContracts.Controls.Add(item);
            }

            // Thêm một spacer panel ở cuối để tạo không gian scroll
            Panel spacerPanel = new Panel();
            spacerPanel.Height = 200; // Tăng chiều cao để đảm bảo có đủ không gian cuộn
            spacerPanel.Width = 1;
            spacerPanel.BackColor = Color.Transparent;
            flowLayoutPanelContracts.Controls.Add(spacerPanel);
        }

        private void Item_Click(object sender, EventArgs e)
        {
            ucContractItem clickedItem = sender as ucContractItem;
            if (clickedItem != null)
            {
                // Reset tất cả hợp đồng về màu bình thường
                foreach (Control control in flowLayoutPanelContracts.Controls)
                {
                    if (control is ucContractItem item)
                    {
                        item.BackColor = Color.FromArgb(236, 250, 236); // Màu nền bình thường
                        foreach (Control childControl in item.Controls)
                        {
                            if (childControl is Label label)
                            {
                                // Reset màu chữ về mặc định
                                if (label.Name == "lblContractID")
                                    label.ForeColor = Color.Black;
                                else if (label.Name == "lblCompanyName")
                                    label.ForeColor = Color.Black;
                                else if (label.Name == "lblDueDate")
                                    label.ForeColor = SystemColors.ControlDarkDark;
                            }
                        }
                    }
                }

                // Đặt hợp đồng được chọn
                selectedContractId = clickedItem.Tag.ToString();
                clickedItem.BackColor = Color.FromArgb(45, 100, 45); // Màu nền khi được chọn

                // Đổi màu chữ thành trắng cho hợp đồng được chọn
                foreach (Control control in clickedItem.Controls)
                {
                    if (control is Label label)
                    {
                        label.ForeColor = Color.White;
                    }
                }

                DisplayContractDetails(selectedContractId);

                var items = contractBLL.GetItemsByContract(selectedContractId);
                if (cmbSamples != null)
                {
                    cmbSamples.DataSource = items;
                    cmbSamples.DisplayMember = "DisplayText";
                    cmbSamples.ValueMember = "ItemId";
                }
                string firstItemId = items.FirstOrDefault()?.ItemId ?? "";
                if (!string.IsNullOrEmpty(firstItemId))
                {
                    var firstItem = items.First();
                    lblSampleTitle.Text = $"Nền mẫu: {firstItem.ItemId} - {firstItem.SampleName}";
                    LoadParametersForItem(firstItem.ItemId);
                }

                txtSearch_TextChanged(txtSearch, EventArgs.Empty);
            }
        }

        private void DisplayContractDetails(string contractID)
        {
            SalesContractDTO details = contractBLL.GetContractDetails(contractID);
            if (details != null)
            {
                txtCompanyName.Text = details.CompanyName;
                txtCustomerID.Text = details.CustomerID;
                txtAddress.Text = details.Address;
                txtRepresentativeName.Text = details.RepresentativeName;
                txtRepresentativeEmail.Text = details.RepresentativeEmail ?? "";
                txtPhoneNumber.Text = details.PhoneNumber;
                
                // Tạm thời unsubscribe event để tránh tự động cập nhật khi load dữ liệu
                dtpSigningDate.ValueChanged -= DtpSigningDate_ValueChanged;
                dtpSigningDate.Value = details.SigningDate;
                dtpExpectedResultDate.Value = details.ExpectedResultDate;
                dtpSigningDate.ValueChanged += DtpSigningDate_ValueChanged; // Subscribe lại

                LoadParametersForContract(details);
            }
        }



        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return ContractInputValidator.IsValidPhoneNumber(phoneNumber);
        }

        private bool IsValidEmail(string email)
        {
            return ContractInputValidator.IsValidEmail(email, allowEmpty: false);
        }

        private bool IsValidLimitRange(string limitRange)
        {
            if (string.IsNullOrEmpty(limitRange))
                return true; // Cho phép để trống

            // Kiểm tra định dạng Min-Max
            if (!limitRange.Contains("-"))
                return false;

            string[] parts = limitRange.Split('-');
            if (parts.Length != 2)
                return false;

            // Kiểm tra cả hai phần đều là số hợp lệ
            decimal min, max;
            return decimal.TryParse(parts[0].Trim(), out min) &&
                   decimal.TryParse(parts[1].Trim(), out max) &&
                   min <= max; // Min phải nhỏ hơn hoặc bằng Max
        }

        private string NormalizeDecimalInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Loại bỏ khoảng trắng
            string cleanInput = input.Replace(" ", "").Trim();

            // Kiểm tra nếu có cả dấu phẩy và dấu chấm
            int commaCount = cleanInput.Count(c => c == ',');
            int dotCount = cleanInput.Count(c => c == '.');

            if (commaCount > 0 && dotCount > 0)
            {
                // Có cả hai dấu - dấu cuối cùng là thập phân
                int lastCommaIndex = cleanInput.LastIndexOf(',');
                int lastDotIndex = cleanInput.LastIndexOf('.');

                if (lastCommaIndex > lastDotIndex)
                {
                    // Dấu phẩy cuối cùng là thập phân
                    cleanInput = cleanInput.Replace(".", "").Replace(",", ".");
                }
                else
                {
                    // Dấu chấm cuối cùng là thập phân
                    cleanInput = cleanInput.Replace(",", "");
                }
            }
            else if (commaCount == 1 && dotCount == 0)
            {
                // Chỉ có một dấu phẩy - kiểm tra số chữ số sau dấu phẩy
                int commaIndex = cleanInput.IndexOf(',');
                string afterComma = cleanInput.Substring(commaIndex + 1);

                // Nếu sau dấu phẩy có 1-2 chữ số và chỉ chứa số, coi là thập phân
                if (afterComma.Length <= 2 && afterComma.All(char.IsDigit))
                {
                    cleanInput = cleanInput.Replace(",", ".");
                }
                else
                {
                    // Coi là phân cách hàng nghìn
                    cleanInput = cleanInput.Replace(",", "");
                }
            }
            else if (commaCount > 1 && dotCount == 0)
            {
                // Nhiều dấu phẩy - coi là phân cách hàng nghìn
                cleanInput = cleanInput.Replace(",", "");
            }
            else if (dotCount > 1 && commaCount == 0)
            {
                // Nhiều dấu chấm - dấu chấm cuối cùng là thập phân
                int lastDotIndex = cleanInput.LastIndexOf('.');
                cleanInput = cleanInput.Substring(0, lastDotIndex).Replace(".", "") +
                           cleanInput.Substring(lastDotIndex);
            }

            return cleanInput;
        }

        private bool IsValidNumericValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true; // Cho phép để trống

            // Chuẩn hóa input trước khi validate
            string normalizedValue = NormalizeDecimalInput(value);

            // Kiểm tra xem có phải là số hợp lệ không, sử dụng CultureInfo.InvariantCulture
            decimal result;
            return decimal.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        private void TxtPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                if (IsValidPhoneNumber(txtPhoneNumber.Text))
                {
                    txtPhoneNumber.FillColor = Color.LightGreen;
                }
                else
                {
                    txtPhoneNumber.FillColor = Color.Red;
                }
            }
            else
            {
                txtPhoneNumber.FillColor = Color.White;
            }
        }

        private void TxtRepresentativeEmail_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRepresentativeEmail.Text))
            {
                if (IsValidEmail(txtRepresentativeEmail.Text))
                {
                    txtRepresentativeEmail.FillColor = Color.LightGreen;
                }
                else
                {
                    txtRepresentativeEmail.FillColor = Color.OrangeRed;
                }
            }
            else
            {
                txtRepresentativeEmail.FillColor = Color.White;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(searchTerm) || searchTerm == "tìm kiếm nhanh..." || searchTerm == "đang lắng nghe...")
            {
                DisplayContracts(fullContractList);
                return;
            }

            var filteredList = fullContractList
                .Where(c => c.ContractID.ToLower().Contains(searchTerm) ||
                            c.CompanyName.ToLower().Contains(searchTerm))
                .ToList();

            DisplayContracts(filteredList);
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm nhanh...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Tìm kiếm nhanh...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void InitializeParameterGrids()
        {
            dgvPlanning.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvFieldLab.EditMode = DataGridViewEditMode.EditOnEnter;

            dgvFieldLab.CellFormatting += dgvFieldLab_CellFormatting;
            dgvPlanning.DataError += (s, e) => { e.ThrowException = false; };
            dgvFieldLab.DataError += (s, e) => { e.ThrowException = false; };

            // Định dạng số cho các cột chứa giá trị số
            FormatNumericColumns();

            ApplyGridColumnPermissions();
        }

        private void FormatNumericColumns()
        {
            // Định dạng cho cột P_LimitRange và P_Result trong dgvPlanning
            if (dgvPlanning.Columns.Contains("P_LimitRange"))
            {
                dgvPlanning.Columns["P_LimitRange"].DefaultCellStyle.Format = "N2";
            }
            if (dgvPlanning.Columns.Contains("P_Result"))
            {
                dgvPlanning.Columns["P_Result"].DefaultCellStyle.Format = "N2";
            }

            // Định dạng cho cột F_LimitRange và F_Result trong dgvFieldLab
            if (dgvFieldLab.Columns.Contains("F_LimitRange"))
            {
                dgvFieldLab.Columns["F_LimitRange"].DefaultCellStyle.Format = "N2";
            }
            if (dgvFieldLab.Columns.Contains("F_Result"))
            {
                dgvFieldLab.Columns["F_Result"].DefaultCellStyle.Format = "N2";
            }
        }

        private void ApplyGridColumnPermissions()
        {
            // Thêm cột Code nếu chưa có (ẩn, chỉ dùng để lưu trữ mã thông số)
            if (!dgvPlanning.Columns.Contains("P_Code"))
            {
                var codeCol = new DataGridViewTextBoxColumn();
                codeCol.Name = "P_Code";
                codeCol.HeaderText = "Mã";
                codeCol.Visible = false; // Ẩn cột này
                codeCol.ReadOnly = true;
                dgvPlanning.Columns.Insert(0, codeCol); // Thêm ở đầu
            }
            if (!dgvFieldLab.Columns.Contains("F_Code"))
            {
                var codeCol = new DataGridViewTextBoxColumn();
                codeCol.Name = "F_Code";
                codeCol.HeaderText = "Mã";
                codeCol.Visible = false; // Ẩn cột này
                codeCol.ReadOnly = true;
                dgvFieldLab.Columns.Insert(0, codeCol); // Thêm ở đầu
            }

            // Các cột luôn read-only
            if (dgvPlanning.Columns.Contains("P_Code")) dgvPlanning.Columns["P_Code"].ReadOnly = true;
            if (dgvPlanning.Columns.Contains("P_Name")) dgvPlanning.Columns["P_Name"].ReadOnly = true;
            if (dgvPlanning.Columns.Contains("P_Unit"))
            {
                dgvPlanning.Columns["P_Unit"].ReadOnly = true;
                dgvPlanning.Columns["P_Unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvFieldLab.Columns.Contains("F_Code")) dgvFieldLab.Columns["F_Code"].ReadOnly = true;
            if (dgvFieldLab.Columns.Contains("F_Name")) dgvFieldLab.Columns["F_Name"].ReadOnly = true;
            if (dgvFieldLab.Columns.Contains("F_Unit"))
            {
                dgvFieldLab.Columns["F_Unit"].ReadOnly = true;
                dgvFieldLab.Columns["F_Unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvFieldLab.Columns.Contains("F_Assignment"))
            {
                dgvFieldLab.Columns["F_Assignment"].ReadOnly = true;
                dgvFieldLab.Columns["F_Assignment"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                var combo = dgvFieldLab.Columns["F_Assignment"] as DataGridViewComboBoxColumn;
                if (combo != null) combo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            }
            if (dgvFieldLab.Columns.Contains("F_LimitRange")) dgvFieldLab.Columns["F_LimitRange"].ReadOnly = true;

            // Thêm cột Result và Note nếu chưa có
            if (!dgvPlanning.Columns.Contains("P_Result")) dgvPlanning.Columns.Add("P_Result", "Kết quả (P.HT/PTN)");
            if (!dgvPlanning.Columns.Contains("P_Note")) dgvPlanning.Columns.Add("P_Note", "Ghi chú (P.HT/PTN)");
            if (!dgvFieldLab.Columns.Contains("F_Result")) dgvFieldLab.Columns.Add("F_Result", "Kết quả (P.HT/PTN)");
            if (!dgvFieldLab.Columns.Contains("F_Note")) dgvFieldLab.Columns.Add("F_Note", "Ghi chú (P.HT/PTN)");

            // Áp dụng phân quyền cho dgvPlanning (Phòng Kế Hoạch)
            if (dgvPlanning.Columns.Contains("P_Assignment"))
            {
                // Chỉ phòng kế hoạch và admin mới có thể chỉnh sửa Phân công
                dgvPlanning.Columns["P_Assignment"].ReadOnly = !PermissionHelper.CanEditAssignment();
                dgvPlanning.Columns["P_Assignment"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvPlanning.Columns.Contains("P_LimitRange"))
            {
                // Chỉ phòng kế hoạch và admin mới có thể chỉnh sửa Giới hạn
                dgvPlanning.Columns["P_LimitRange"].ReadOnly = !PermissionHelper.CanEditLimitRange();
            }

            // Kết quả và ghi chú trong dgvPlanning luôn read-only (chỉ phòng hiện trường/thí nghiệm mới nhập)
            dgvPlanning.Columns["P_Result"].ReadOnly = true;
            dgvPlanning.Columns["P_Result"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanning.Columns["P_Note"].ReadOnly = true;
            dgvPlanning.Columns["P_Result"].Visible = true;
            dgvPlanning.Columns["P_Note"].Visible = true;

            // Áp dụng phân quyền cho dgvFieldLab (Phòng Hiện Trường/Thí Nghiệm)
            // Phòng hiện trường và thí nghiệm chỉ có thể nhập kết quả đo đạt
            if (dgvFieldLab.Columns.Contains("F_Result"))
            {
                dgvFieldLab.Columns["F_Result"].ReadOnly = !PermissionHelper.CanEditResults();
                dgvFieldLab.Columns["F_Result"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvFieldLab.Columns.Contains("F_Note"))
            {
                dgvFieldLab.Columns["F_Note"].ReadOnly = !PermissionHelper.CanEditResults();
            }

            dgvFieldLab.Columns["F_Result"].Visible = true;
            dgvFieldLab.Columns["F_Note"].Visible = true;

            // Phòng Kết Quả chỉ có thể xem, không chỉnh sửa
            if (PermissionHelper.IsDepartment("Phòng Kết Quả") && !PermissionHelper.IsAdmin())
            {
                dgvPlanning.ReadOnly = true;
                dgvFieldLab.ReadOnly = true;
            }
            else
            {
                dgvPlanning.ReadOnly = false;
                dgvFieldLab.ReadOnly = false;
            }

            dgvFieldLab.AllowUserToAddRows = false;
            dgvPlanning.AllowUserToAddRows = false;
        }

        private void LoadParametersForContract(SalesContractDTO contract)
        {
            var items = contractBLL.GetItemsByContract(contract.ContractID);
            string itemId = items.FirstOrDefault()?.ItemId ?? string.Empty;
            lblSampleTitle.Text = !string.IsNullOrEmpty(itemId) ? $"Nền mẫu: {itemId} - {contract.Address}" : "Nền mẫu: (không có)";

            LoadParametersForItem(itemId);
        }

        private void LoadParametersForItem(string itemId)
        {
            var paramBll = new ContractParameterBLL();
            var plan = string.IsNullOrEmpty(itemId) ? new List<DTO.ParameterPlanDTO>() : paramBll.GetPlanningParameters(itemId);
            var results = string.IsNullOrEmpty(itemId) ? new List<DTO.ParameterResultDTO>() : paramBll.GetFieldLabParametersWithResults(itemId);

            // Lấy SampleType từ Item - nhưng cho phép mỗi thông số có giá trị riêng
            // Chỉ dùng itemSampleType làm giá trị mặc định ban đầu
            string itemSampleType = plan.FirstOrDefault()?.SampleType ?? results.FirstOrDefault()?.SampleType ?? string.Empty;

            // Nếu không có từ parameter data, lấy trực tiếp từ ContractItem
            if (string.IsNullOrEmpty(itemSampleType) && !string.IsNullOrEmpty(itemId))
            {
                // Lấy contractId từ selectedItem hoặc từ ItemId đã có
                string contractId = null;
                if (cmbSamples?.SelectedItem is DTO.ContractItemDTO selectedItem)
                {
                    contractId = selectedItem.ContractId;
                }
                else if (!string.IsNullOrEmpty(selectedContractId))
                {
                    contractId = selectedContractId;
                }

                if (!string.IsNullOrEmpty(contractId))
                {
                    var items = contractBLL.GetItemsByContract(contractId);
                    var currentItem = items?.FirstOrDefault(i => i.ItemId == itemId);
                    itemSampleType = currentItem?.SampleType ?? string.Empty;
                }
            }

            // Lưu tất cả các thông số (chưa lọc) để dùng cho filtering
            // Tạo map kết quả từ FieldLab để hiển thị trong Planning
            var resultMap = results.ToDictionary(x => x.ParameterId, x => x);

            var allPlanningRows = plan.Select(p => new ParameterRow
            {
                Code = p.ParameterId,
                Name = p.ParameterName,
                Unit = p.UnitSymbol,
                Assignment = p.AssignmentCode,
                SampleType = p.SampleType ?? itemSampleType, // Lấy từ parameter trước, nếu không có thì dùng từ item
                LimitRange = (p.MinValue.HasValue || p.MaxValue.HasValue) ? $"{p.MinValue?.ToString("N2") ?? ""}-{p.MaxValue?.ToString("N2") ?? ""}" : string.Empty,
                Result = resultMap.TryGetValue(p.ParameterId, out var res) && res.Value.HasValue ? res.Value.Value.ToString("N2") : null,
                Note = resultMap.TryGetValue(p.ParameterId, out var res2) && !string.IsNullOrWhiteSpace(res2.Note) ? res2.Note : p.Note
            }).ToList();

            var allFieldLabRows = results.Select(p => new ParameterRow
            {
                Code = p.ParameterId,
                Name = p.ParameterName,
                Unit = p.UnitSymbol,
                Assignment = p.AssignmentCode,
                SampleType = p.SampleType ?? itemSampleType, // Lấy từ parameter trước, nếu không có thì dùng từ item
                LimitRange = (p.MinValue.HasValue || p.MaxValue.HasValue) ? $"{p.MinValue?.ToString("N2") ?? ""}-{p.MaxValue?.ToString("N2") ?? ""}" : string.Empty,
                Result = p.Value?.ToString("N2"),
                Note = p.Note
            }).ToList();

            // Populate combobox loại mẫu với các giá trị có sẵn trong thông số
            if (cmbItemSampleType != null)
            {
                var sampleTypes = allPlanningRows.Select(r => r.SampleType)
                    .Concat(allFieldLabRows.Select(r => r.SampleType))
                    .Where(st => !string.IsNullOrEmpty(st))
                    .Distinct()
                    .OrderBy(st => st)
                    .ToList();

                // Thêm "Tất cả" vào đầu danh sách
                sampleTypes.Insert(0, "Tất cả");

                // Bảo đảm luôn có các loại chuẩn để lọc
                foreach (var t in new[] { "Khí thải", "Nước thải", "Không khí", "Tiếng ồn" })
                {
                    if (!sampleTypes.Contains(t)) sampleTypes.Add(t);
                }

                cmbItemSampleType.DataSource = sampleTypes;

                // Mặc định luôn hiển thị "Tất cả" để không lọc ngay khi mở
                cmbItemSampleType.SelectedItem = "Tất cả";
            }

            // Lưu để dùng cho filtering
            this.allPlanningRows = allPlanningRows;
            this.allFieldLabRows = allFieldLabRows;

            // Áp dụng filter và hiển thị
            ApplySampleTypeFilter(allPlanningRows, allFieldLabRows);

            // Áp dụng phân quyền cho các cột sau khi load dữ liệu
            ApplyGridColumnPermissions();
        }

        private void btnSavePlanning_Click(object sender, EventArgs e)
        {
            string itemId = lblSampleTitle.Text.StartsWith("Nền mẫu: ") ? lblSampleTitle.Text.Split(' ')[2] : null;
            var bll = new ContractParameterBLL();

            // Validation cho các trường giới hạn (Limit Range)
            for (int i = 0; i < dgvPlanning.Rows.Count; i++)
            {
                var row = dgvPlanning.Rows[i];
                if (row.IsNewRow) continue;

                string limitRange = Convert.ToString(row.Cells["P_LimitRange"].Value);
                if (!string.IsNullOrEmpty(limitRange))
                {
                    if (!IsValidLimitRange(limitRange))
                    {
                        string parameterName = Convert.ToString(row.Cells["P_Name"].Value);
                        MessageBox.Show($"Giới hạn của thông số '{parameterName}' không đúng định dạng.\nĐịnh dạng đúng: Min-Max (ví dụ: 10.5-20.3)", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Focus vào ô lỗi
                        dgvPlanning.CurrentCell = row.Cells["P_LimitRange"];
                        dgvPlanning.BeginEdit(true);
                        return;
                    }
                }
            }

            // Lưu các thiết lập cho từng thông số (sử dụng allPlanningRows để lưu tất cả, không chỉ những gì đang hiển thị)
            foreach (var originalRow in allPlanningRows)
            {
                // Tìm row tương ứng trong dgvPlanning (nếu có chỉnh sửa)
                var displayedRow = dgvPlanning.Rows.Cast<DataGridViewRow>()
                    .FirstOrDefault(r => !r.IsNewRow && Convert.ToString(r.Cells["P_Code"].Value) == originalRow.Code);

                if (displayedRow != null)
                {
                    // Có chỉnh sửa trên row này
                    string parameterId = Convert.ToString(displayedRow.Cells["P_Code"].Value);
                    var assignVal = Convert.ToString(displayedRow.Cells["P_Assignment"].Value);
                    if (!string.IsNullOrWhiteSpace(assignVal))
                    {
                        new ContractParameterBLL().SaveAssignment(itemId, parameterId, assignVal);
                    }

                    string limit = Convert.ToString(displayedRow.Cells["P_LimitRange"].Value);
                    decimal? min = null; decimal? max = null;
                    if (!string.IsNullOrWhiteSpace(limit))
                    {
                        var parts = limit.Split('-');
                        if (parts.Length == 2)
                        {
                            decimal temp;
                            if (decimal.TryParse(parts[0], out temp)) min = temp;
                            if (decimal.TryParse(parts[1], out temp)) max = temp;
                        }
                    }

                    string note = dgvPlanning.Columns.Contains("P_Note") ? Convert.ToString(displayedRow.Cells["P_Note"].Value) : null;
                    bll.SavePlanningSetting(itemId, parameterId, min, max, note, null);
                }
                else
                {
                    // Row này không được hiển thị (bị filter), lưu giá trị gốc
                    string parameterId = originalRow.Code;
                    var assignVal = originalRow.Assignment;
                    if (!string.IsNullOrWhiteSpace(assignVal))
                    {
                        new ContractParameterBLL().SaveAssignment(itemId, parameterId, assignVal);
                    }

                    string limit = originalRow.LimitRange;
                    decimal? min = null; decimal? max = null;
                    if (!string.IsNullOrWhiteSpace(limit))
                    {
                        var parts = limit.Split('-');
                        if (parts.Length == 2)
                        {
                            decimal temp;
                            if (decimal.TryParse(parts[0], out temp)) min = temp;
                            if (decimal.TryParse(parts[1], out temp)) max = temp;
                        }
                    }

                    string note = originalRow.Note;
                    bll.SavePlanningSetting(itemId, parameterId, min, max, note, null);
                }
            }

            MessageBox.Show("Đã lưu phân công/giới hạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadParametersForItem(itemId);
        }

        private void btnSaveResults_Click(object sender, EventArgs e)
        {
            string itemId = lblSampleTitle.Text.StartsWith("Nền mẫu: ") ? lblSampleTitle.Text.Split(' ')[2] : null;
            var bll = new ContractParameterBLL();
            string batchId = bll.GetLatestBatchId(itemId);
            if (string.IsNullOrEmpty(batchId))
            {
                MessageBox.Show("Chưa có lô đo cho nền mẫu này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation cho các trường kết quả
            for (int i = 0; i < dgvFieldLab.Rows.Count; i++)
            {
                var row = dgvFieldLab.Rows[i];
                if (row.IsNewRow) continue;

                string resultValue = Convert.ToString(row.Cells["F_Result"].Value);
                if (!string.IsNullOrEmpty(resultValue))
                {
                    // Chuẩn hóa input trước khi validate
                    string normalizedValue = NormalizeDecimalInput(resultValue);

                    if (!IsValidNumericValue(normalizedValue))
                    {
                        string parameterName = Convert.ToString(row.Cells["F_Name"].Value);
                        MessageBox.Show($"Kết quả của thông số '{parameterName}' phải là số hợp lệ.\nVui lòng nhập lại.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Focus vào ô lỗi
                        dgvFieldLab.CurrentCell = row.Cells["F_Result"];
                        dgvFieldLab.BeginEdit(true);
                        return;
                    }
                }
            }

            for (int i = 0; i < dgvFieldLab.Rows.Count; i++)
            {
                var row = dgvFieldLab.Rows[i];
                if (row.IsNewRow) continue;

                string parameterId = Convert.ToString(row.Cells["F_Code"].Value);
                decimal? value = null;
                decimal tmp;
                string resultValue = Convert.ToString(row.Cells["F_Result"].Value);
                if (!string.IsNullOrEmpty(resultValue))
                {
                    // Chuẩn hóa input trước khi parse
                    string normalizedValue = NormalizeDecimalInput(resultValue);
                    if (decimal.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                        value = tmp;
                }

                string note = Convert.ToString(row.Cells["F_Note"].Value);

                // Tự động cập nhật ghi chú dựa trên kết quả và giới hạn
                if (value.HasValue)
                {
                    string limitRange = Convert.ToString(row.Cells["F_LimitRange"].Value);
                    if (!string.IsNullOrEmpty(limitRange) && limitRange.Contains("-"))
                    {
                        string[] limits = limitRange.Split('-');
                        if (limits.Length == 2)
                        {
                            decimal minLimit, maxLimit;
                            if (decimal.TryParse(limits[0], out minLimit) && decimal.TryParse(limits[1], out maxLimit))
                            {
                                if (value >= minLimit && value <= maxLimit)
                                {
                                    note = "Đạt";
                                }
                                else
                                {
                                    note = "Không đạt";
                                }
                                // Cập nhật lại giá trị trong grid
                                row.Cells["F_Note"].Value = note;
                            }
                        }
                    }
                }

                bll.SaveResult(batchId, parameterId, value, note, null);
            }

            MessageBox.Show("Đã lưu kết quả/ghi chú.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadParametersForItem(itemId);
            // Phát sự kiện để các khu vực khác cập nhật real-time (trường hợp kết quả ảnh hưởng trạng thái)
            GlobalEventBus.RaiseContractDataChanged();
        }

        private void btnAddSampleItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedContractId))
            {
                MessageBox.Show("Vui lòng chọn hợp đồng trước.");
                return;
            }

            using (var dlg = new Form())
            {
                dlg.Text = "Thêm nền mẫu";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.ClientSize = new Size(420, 250);
                dlg.MinimizeBox = false;
                dlg.MaximizeBox = false;

                var lblName = new Label { Text = "Tên nền mẫu", Left = 16, Top = 20, Width = 380 };
                var txtName = new TextBox { Left = 16, Top = 50, Width = 380 };
                
                var lblType = new Label { Text = "Loại mẫu *", Left = 16, Top = 85, Width = 380 };
                var cmbType = new ComboBox 
                { 
                    Left = 16, 
                    Top = 115, 
                    Width = 380, 
                    DropDownStyle = ComboBoxStyle.DropDownList 
                };
                cmbType.Items.AddRange(new[] { "Khí thải", "Nước thải", "Không khí", "Tiếng ồn" });
                
                var lblLoc = new Label { Text = "Vị trí (tuỳ chọn)", Left = 16, Top = 150, Width = 380 };
                var txtLoc = new TextBox { Left = 16, Top = 180, Width = 380 };
                
                var btnOK = new Button { Text = "Thêm", DialogResult = DialogResult.OK, Left = 220, Width = 80, Top = 220 };
                var btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Left = 316, Width = 80, Top = 220 };
                
                dlg.Controls.Add(lblName); dlg.Controls.Add(txtName);
                dlg.Controls.Add(lblType); dlg.Controls.Add(cmbType);
                dlg.Controls.Add(lblLoc); dlg.Controls.Add(txtLoc);
                dlg.Controls.Add(btnOK); dlg.Controls.Add(btnCancel);

                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                
                string name = txtName.Text?.Trim();
                string sampleType = cmbType.SelectedItem?.ToString();
                string location = txtLoc.Text?.Trim();
                
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Tên nền mẫu không được trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(sampleType))
                {
                    MessageBox.Show("Vui lòng chọn loại mẫu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string newItemId = contractBLL.AddContractItem(selectedContractId, name, sampleType, location);
                if (string.IsNullOrEmpty(newItemId))
                {
                    MessageBox.Show("Thêm nền mẫu thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Reload danh sách nền mẫu và chọn nền mới
                var items = contractBLL.GetItemsByContract(selectedContractId);
                cmbSamples.DataSource = items;
                cmbSamples.DisplayMember = "DisplayText";
                cmbSamples.ValueMember = "ItemId";
                var added = items.FirstOrDefault(i => i.ItemId == newItemId);
                if (added != null)
                {
                    cmbSamples.SelectedItem = added;
                    lblSampleTitle.Text = $"Nền mẫu: {added.ItemId} - {added.SampleName}";
                    LoadParametersForItem(added.ItemId);
                }
            }
        }

        private void btnDeleteSampleItem_Click(object sender, EventArgs e)
        {
            if (!(cmbSamples?.SelectedItem is DTO.ContractItemDTO selectedItem))
            {
                MessageBox.Show("Vui lòng chọn nền mẫu cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nền mẫu '{selectedItem.ItemId} - {selectedItem.SampleName}'?\n\n" +
                "Lưu ý: Tất cả dữ liệu liên quan (thông số, kết quả, lô đo) sẽ bị xóa theo.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            // Thực hiện xóa
            bool success = contractBLL.DeleteContractItem(selectedItem.ItemId);
            if (success)
            {
                MessageBox.Show("Đã xóa nền mẫu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Reload danh sách nền mẫu
                if (!string.IsNullOrEmpty(selectedContractId))
                {
                    var items = contractBLL.GetItemsByContract(selectedContractId);
                    cmbSamples.DataSource = items;
                    cmbSamples.DisplayMember = "DisplayText";
                    cmbSamples.ValueMember = "ItemId";
                    
                    // Chọn nền mẫu đầu tiên nếu còn
                    if (items != null && items.Count > 0)
                    {
                        cmbSamples.SelectedIndex = 0;
                        var firstItem = items[0];
                        lblSampleTitle.Text = $"Nền mẫu: {firstItem.ItemId} - {firstItem.SampleName}";
                        LoadParametersForItem(firstItem.ItemId);
                    }
                    else
                    {
                        lblSampleTitle.Text = "Nền mẫu: (Chưa có)";
                        dgvPlanning.Rows.Clear();
                        dgvFieldLab.Rows.Clear();
                    }
                }
            }
            else
            {
                MessageBox.Show("Xóa nền mẫu thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateBatch_Click(object sender, EventArgs e)
        {
            if (!(cmbSamples?.SelectedItem is DTO.ContractItemDTO selectedItem))
            {
                MessageBox.Show("Vui lòng chọn nền mẫu trước.");
                return;
            }

            using (var dlg = new Form())
            {
                dlg.Text = "Tạo lô đo";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.ClientSize = new Size(380, 160);
                dlg.MinimizeBox = false; dlg.MaximizeBox = false;

                var lbl = new Label { Text = "Ngày bắt đầu", Left = 16, Top = 20, Width = 150 };
                var dtp = new DateTimePicker { Left = 16, Top = 50, Width = 330, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
                var ok = new Button { Text = "Tạo", DialogResult = DialogResult.OK, Left = 180, Width = 75, Top = 105 };
                var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Left = 270, Width = 75, Top = 105 };
                dlg.Controls.AddRange(new Control[] { lbl, dtp, ok, cancel });

                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var bll = new ContractParameterBLL();
                string newBatchId = bll.CreateMeasurementBatch(selectedItem.ItemId, dtp.Value, null);
                if (string.IsNullOrEmpty(newBatchId))
                {
                    MessageBox.Show("Tạo lô đo thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show($"Đã tạo lô đo: {newBatchId}");
                LoadParametersForItem(selectedItem.ItemId);
            }
        }

        private void btnAddParameter_Click(object sender, EventArgs e)
        {
            if (!(cmbSamples?.SelectedItem is DTO.ContractItemDTO selectedItem))
            {
                MessageBox.Show("Vui lòng chọn nền mẫu trước.");
                return;
            }

            var paramBll = new ContractParameterBLL();
            var avail = paramBll.GetUnassignedParameters(selectedItem.ItemId);
            if (avail == null || avail.Count == 0)
            {
                MessageBox.Show("Không còn thông số nào để thêm.");
                return;
            }

            using (var dlg = new Form())
            {
                dlg.Text = "Chọn thông số cần thêm";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MinimizeBox = false;
                dlg.MaximizeBox = false;
                dlg.ClientSize = new Size(520, 520);

                var clb = new CheckedListBox { Dock = DockStyle.Top, Height = 380, CheckOnClick = true, Font = new System.Drawing.Font("Segoe UI", 10F) };
                foreach (var p in avail)
                {
                    var display = $"{p.ParameterId} - {p.ParameterName} ({p.UnitSymbol})";
                    clb.Items.Add(display, false);
                }

                var lblAssign = new Label { Text = "Phân công:", Dock = DockStyle.Top, Height = 28, TextAlign = ContentAlignment.MiddleLeft };
                var cboAssign = new ComboBox { Dock = DockStyle.Top, Height = 32, DropDownStyle = ComboBoxStyle.DropDownList };
                cboAssign.Items.AddRange(new object[] { "HT", "PT" });
                cboAssign.SelectedIndex = 0;

                var panelButtons = new Panel { Dock = DockStyle.Bottom, Height = 60 };
                var btnOK = new Button { Text = "Thêm", DialogResult = DialogResult.OK, Width = 100, Height = 36, Left = 520 - 220, Top = 12 };
                var btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 100, Height = 36, Left = 520 - 110, Top = 12 };
                panelButtons.Controls.Add(btnOK);
                panelButtons.Controls.Add(btnCancel);

                dlg.Controls.Add(panelButtons);
                dlg.Controls.Add(cboAssign);
                dlg.Controls.Add(lblAssign);
                dlg.Controls.Add(clb);

                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var selectedIds = new List<string>();
                foreach (var item in clb.CheckedItems)
                {
                    string text = item.ToString();
                    int dash = text.IndexOf(' ');
                    string id = dash > 0 ? text.Substring(0, dash) : text;
                    selectedIds.Add(id);
                }
                if (selectedIds.Count == 0)
                {
                    MessageBox.Show("Bạn chưa chọn thông số nào.");
                    return;
                }

                string assign = cboAssign.SelectedItem?.ToString() ?? "HT";
                foreach (var pid in selectedIds)
                {
                    paramBll.SaveAssignment(selectedItem.ItemId, pid, assign);
                    paramBll.SavePlanningSetting(selectedItem.ItemId, pid, null, null, null, null, null);
                }

                LoadParametersForItem(selectedItem.ItemId);
                MessageBox.Show("Đã thêm thông số.");
            }
        }

        private void btnDeleteParameter_Click(object sender, EventArgs e)
        {
            if (!(cmbSamples?.SelectedItem is DTO.ContractItemDTO selectedItem))
            {
                MessageBox.Show("Vui lòng chọn nền mẫu trước.");
                return;
            }

            var paramBll = new ContractParameterBLL();
            var assigned = paramBll.GetPlanningParameters(selectedItem.ItemId);
            if (assigned == null || assigned.Count == 0)
            {
                MessageBox.Show("Không có thông số nào để xóa.");
                return;
            }

            using (var dlg = new Form())
            {
                dlg.Text = "Chọn thông số cần xóa";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MinimizeBox = false;
                dlg.MaximizeBox = false;
                dlg.ClientSize = new Size(520, 450);

                var clb = new CheckedListBox { Dock = DockStyle.Top, Height = 350, CheckOnClick = true, Font = new System.Drawing.Font("Segoe UI", 10F) };
                foreach (var p in assigned)
                {
                    var display = $"{p.ParameterId} - {p.ParameterName} ({p.UnitSymbol}) - Phân công: {p.AssignmentCode}";
                    clb.Items.Add(display, false);
                }

                var panelButtons = new Panel { Dock = DockStyle.Bottom, Height = 60 };
                var btnOK = new Button { Text = "Xóa", DialogResult = DialogResult.OK, Width = 100, Height = 36, Left = 520 - 220, Top = 12 };
                var btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 100, Height = 36, Left = 520 - 110, Top = 12 };
                panelButtons.Controls.Add(btnOK);
                panelButtons.Controls.Add(btnCancel);

                dlg.Controls.Add(panelButtons);
                dlg.Controls.Add(clb);

                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var selectedIds = new List<string>();
                foreach (var item in clb.CheckedItems)
                {
                    string text = item.ToString();
                    int dash = text.IndexOf(' ');
                    string id = dash > 0 ? text.Substring(0, dash) : text;
                    selectedIds.Add(id);
                }
                if (selectedIds.Count == 0)
                {
                    MessageBox.Show("Bạn chưa chọn thông số nào để xóa.");
                    return;
                }

                // Xác nhận trước khi xóa
                var confirmResult = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa {selectedIds.Count} thông số đã chọn?\nThao tác này sẽ xóa cả dữ liệu kết quả liên quan (nếu có).",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes) return;

                int successCount = 0;
                foreach (var pid in selectedIds)
                {
                    if (paramBll.DeleteAssignment(selectedItem.ItemId, pid))
                    {
                        successCount++;
                    }
                }

                LoadParametersForItem(selectedItem.ItemId);
                MessageBox.Show($"Đã xóa {successCount}/{selectedIds.Count} thông số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvFieldLab_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvFieldLab.Columns[e.ColumnIndex].Name == "F_Result")
            {
                var row = dgvFieldLab.Rows[e.RowIndex];
                string limit = Convert.ToString(row.Cells["F_LimitRange"].Value);
                if (string.IsNullOrWhiteSpace(limit)) return;
                string[] parts = limit.Split('-');
                if (parts.Length != 2) return;

                // Chuẩn hóa input trước khi parse
                string limitMin = NormalizeDecimalInput(parts[0]);
                string limitMax = NormalizeDecimalInput(parts[1]);

                // Chuẩn hóa giá trị kết quả trước khi parse
                string normalizedResult = NormalizeDecimalInput(Convert.ToString(e.Value));

                if (double.TryParse(normalizedResult, NumberStyles.Any, CultureInfo.InvariantCulture, out double result) &&
                    double.TryParse(limitMin, NumberStyles.Any, CultureInfo.InvariantCulture, out double min) &&
                    double.TryParse(limitMax, NumberStyles.Any, CultureInfo.InvariantCulture, out double max))
                {
                    if (result >= max || result <= min)
                    {
                        row.Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                        row.Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                        if (string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["F_Note"].Value)))
                        {
                            row.Cells["F_Note"].Value = "Không đạt";
                        }
                    }
                    else
                    {
                        row.Cells[e.ColumnIndex].Style.BackColor = Color.White;
                        row.Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                        if (string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["F_Note"].Value)))
                        {
                            row.Cells["F_Note"].Value = "Đạt";
                        }
                    }
                }
            }
        }

        private void dgvFieldLab_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Tự động cập nhật ghi chú khi người dùng nhập kết quả
            if (dgvFieldLab.Columns[e.ColumnIndex].Name == "F_Result")
            {
                var row = dgvFieldLab.Rows[e.RowIndex];
                if (row.IsNewRow) return;

                string resultValue = Convert.ToString(row.Cells["F_Result"].Value);
                string limitRange = Convert.ToString(row.Cells["F_LimitRange"].Value);

                if (!string.IsNullOrEmpty(resultValue) && !string.IsNullOrEmpty(limitRange) && limitRange.Contains("-"))
                {
                    // Chuẩn hóa input trước khi parse
                    string cleanResultValue = NormalizeDecimalInput(resultValue);
                    string[] limits = limitRange.Split('-');
                    if (limits.Length == 2)
                    {
                        string cleanMinLimit = NormalizeDecimalInput(limits[0]);
                        string cleanMaxLimit = NormalizeDecimalInput(limits[1]);

                        decimal result, minLimit, maxLimit;
                        if (decimal.TryParse(cleanResultValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result) &&
                            decimal.TryParse(cleanMinLimit, NumberStyles.Any, CultureInfo.InvariantCulture, out minLimit) &&
                            decimal.TryParse(cleanMaxLimit, NumberStyles.Any, CultureInfo.InvariantCulture, out maxLimit))
                        {
                            string note;
                            if (result >= minLimit && result <= maxLimit)
                            {
                                note = "Đạt";
                            }
                            else
                            {
                                note = "Không đạt";
                            }

                            // Cập nhật ghi chú tự động
                            row.Cells["F_Note"].Value = note;
                        }
                    }
                }
            }
        }

        private void panelParametersScroll_Paint(object sender, PaintEventArgs e) { }
        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                // 1) Tạo thư mục xuất
                string exportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);

                // 2) Tên file PDF (theo hợp đồng đang chọn)
                string contractId = selectedContractId ?? "UNKNOWN";
                string pdfPath = Path.Combine(exportDir, $"BaoCao_{contractId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

                // 3) Tạo PDF tối giản với tiêu đề ASCII để tránh lỗi font
                CreateContractPdf(pdfPath, contractId);

                if (!File.Exists(pdfPath))
                {
                    MessageBox.Show("Không thể tạo file PDF.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 4) Tự động mở file sau khi tạo
                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                });

                MessageBox.Show("Đã tạo báo cáo PDF và mở tệp thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void btnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string contractId = selectedContractId ?? "UNKNOWN";
                
                // Lấy email đại diện khách hàng trên tab Thông tin chung
                string toEmail = txtRepresentativeEmail?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(toEmail))
                {
                    MessageBox.Show("Chưa có email khách hàng. Vui lòng bổ sung ở tab Thông tin chung.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo nhiều file PDF, mỗi file cho một nền mẫu
                this.UseWaitCursor = true;
                btnSendEmail.Enabled = false;
                btnPrintReport.Enabled = false;

                List<string> pdfPaths = CreateContractPdfs(contractId);
                
                if (pdfPaths == null || pdfPaths.Count == 0)
                {
                    MessageBox.Show("Không có file PDF nào được tạo. Vui lòng kiểm tra lại dữ liệu hợp đồng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.UseWaitCursor = false;
                    btnSendEmail.Enabled = true;
                    btnPrintReport.Enabled = true;
                    return;
                }

                // Khởi tạo EmailService từ App.config
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                string smtpUsername = ConfigurationManager.AppSettings["FromEmail"];
                string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
                string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                var emailService = new BLL.EmailService(smtpHost, smtpPort, smtpUsername, smtpPassword, enableSsl, fromEmail);

                string subject = $"[EcoGreen] Báo cáo hoàn tất hợp đồng {contractId}";
                string body = $"Chào Quý Khách,<br/><br/>Hợp đồng <b>{contractId}</b> đã được hoàn tất. " +
                              $"Vui lòng xem {pdfPaths.Count} báo cáo đính kèm (mỗi báo cáo tương ứng với một nền mẫu).<br/><br/>Trân trọng,<br/>EcoGreen";

                // Gửi email với tất cả các file PDF đính kèm
                bool ok = await emailService.SendEmailAsync(toEmail, subject, body, pdfPaths);
                
                this.UseWaitCursor = false;
                btnSendEmail.Enabled = true;
                btnPrintReport.Enabled = true;
                
                if (ok)
                {
                    MessageBox.Show($"Đã gửi email cho khách hàng kèm {pdfPaths.Count} báo cáo PDF.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể gửi email. Vui lòng kiểm tra cấu hình SMTP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.UseWaitCursor = false;
                btnSendEmail.Enabled = true;
                btnPrintReport.Enabled = true;
                MessageBox.Show($"Lỗi khi gửi email: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvPlanning_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void dgvPlanning_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Không có logic đồng bộ - để mỗi cell độc lập hoàn toàn
        }

        private List<ParameterRow> allPlanningRows = new List<ParameterRow>();
        private List<ParameterRow> allFieldLabRows = new List<ParameterRow>();

        private void CmbItemSampleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySampleTypeFilter(allPlanningRows, allFieldLabRows);
        }

        // Hàm xác định thông số thuộc loại mẫu nào
        private List<string> GetSampleTypesForParameter(string parameterId)
        {
            // Mapping thông số theo loại mẫu
            var parameterSampleTypeMap = new Dictionary<string, List<string>>
            {
                // Thông số khí thải
                { "TS004", new List<string> { "Khí thải" } }, // CO
                { "TS005", new List<string> { "Khí thải" } }, // NOx
                { "TS006", new List<string> { "Khí thải" } }, // SO2
                { "TS007", new List<string> { "Khí thải" } }, // Bụi tổng
                { "TS008", new List<string> { "Khí thải" } }, // Pb
                { "TS009", new List<string> { "Khí thải" } }, // Cd
                { "TS010", new List<string> { "Khí thải" } }, // As

                // Thông số nước thải (NTxxx)
                { "NT005", new List<string> { "Nước thải" } }, // pH
                { "NT006", new List<string> { "Nước thải" } }, // BOD5
                { "NT007", new List<string> { "Nước thải" } }, // COD
                { "NT008", new List<string> { "Nước thải" } }, // TSS
                { "NT009", new List<string> { "Nước thải" } }, // NH3-N
                { "NT010", new List<string> { "Nước thải" } }, // Tổng N
                { "NT011", new List<string> { "Nước thải" } }, // Tổng P
                { "NT012", new List<string> { "Nước thải" } }, // Dầu mỡ
                { "NT013", new List<string> { "Nước thải" } }, // Kim loại nặng
                { "NT015", new List<string> { "Nước thải" } }, // Coliform

                // Thông số tiếng ồn (chi tiết)
                { "TN001", new List<string> { "Tiếng ồn" } }, // Ban ngày
                { "TN002", new List<string> { "Tiếng ồn" } }, // Ban đêm
                { "TS002", new List<string> { "Tiếng ồn" } },  // mapping tổng hợp cũ -> Tiếng ồn

                // Thông số không khí xung quanh
                { "KK001", new List<string> { "Không khí" } },
                { "KK002", new List<string> { "Không khí" } },
                { "KK003", new List<string> { "Không khí" } },
                { "KK004", new List<string> { "Không khí" } },
                { "KK005", new List<string> { "Không khí" } },
            };

            // Nếu có trong map thì trả về danh sách loại mẫu
            if (parameterSampleTypeMap.ContainsKey(parameterId))
            {
                return parameterSampleTypeMap[parameterId];
            }

            // Mặc định: nếu không có trong map, không thuộc loại mẫu cụ thể nào (chỉ hiển thị khi chọn "Tất cả")
            return new List<string>();
        }

        private void ApplySampleTypeFilter(List<ParameterRow> allPlanning, List<ParameterRow> allFieldLab)
        {
            string selectedSampleType = cmbItemSampleType?.SelectedItem?.ToString();

            // Lọc theo loại mẫu được chọn
            if (string.IsNullOrEmpty(selectedSampleType) || selectedSampleType == "Tất cả")
            {
                planningRows = allPlanning.ToList();
                fieldLabRows = allFieldLab.ToList();
            }
            else
            {
                // Lọc dựa trên mapping thông số - loại mẫu
                planningRows = allPlanning.Where(r =>
                {
                    var sampleTypes = GetSampleTypesForParameter(r.Code);
                    return sampleTypes.Contains(selectedSampleType);
                }).ToList();

                fieldLabRows = allFieldLab.Where(r =>
                {
                    var sampleTypes = GetSampleTypesForParameter(r.Code);
                    return sampleTypes.Contains(selectedSampleType);
                }).ToList();
            }

            // Gọi ApplyGridColumnPermissions() để đảm bảo tất cả cột tồn tại
            ApplyGridColumnPermissions();

            // Hiển thị dữ liệu đã lọc vào DataGridView
            dgvPlanning.Rows.Clear();
            foreach (var r in planningRows)
            {
                int idx = dgvPlanning.Rows.Add();
                if (dgvPlanning.Columns.Contains("P_Code"))
                    dgvPlanning.Rows[idx].Cells["P_Code"].Value = r.Code;
                dgvPlanning.Rows[idx].Cells["P_Name"].Value = r.Name;
                dgvPlanning.Rows[idx].Cells["P_Unit"].Value = r.Unit;
                dgvPlanning.Rows[idx].Cells["P_Assignment"].Value = r.Assignment;
                dgvPlanning.Rows[idx].Cells["P_LimitRange"].Value = r.LimitRange;
                if (dgvPlanning.Columns.Contains("P_Result"))
                {
                    dgvPlanning.Rows[idx].Cells["P_Result"].Value = r.Result;
                }
                if (dgvPlanning.Columns.Contains("P_Note"))
                {
                    dgvPlanning.Rows[idx].Cells["P_Note"].Value = r.Note;
                }
            }

            dgvFieldLab.Rows.Clear();
            foreach (var r in fieldLabRows)
            {
                int idx2 = dgvFieldLab.Rows.Add();
                if (dgvFieldLab.Columns.Contains("F_Code"))
                    dgvFieldLab.Rows[idx2].Cells["F_Code"].Value = r.Code;
                dgvFieldLab.Rows[idx2].Cells["F_Name"].Value = r.Name;
                dgvFieldLab.Rows[idx2].Cells["F_Unit"].Value = r.Unit;
                dgvFieldLab.Rows[idx2].Cells["F_Assignment"].Value = r.Assignment;
                dgvFieldLab.Rows[idx2].Cells["F_LimitRange"].Value = r.LimitRange;
                dgvFieldLab.Rows[idx2].Cells["F_Result"].Value = r.Result;
                dgvFieldLab.Rows[idx2].Cells["F_Note"].Value = r.Note;
            }

            // Áp dụng lại phân quyền sau khi populate data
            ApplyGridColumnPermissions();
        }


        #endregion


        // Tạo PDF báo cáo hợp đồng theo mẫu phiếu kết quả thử nghiệm
        private void CreateContractPdf(string outputPath, string contractId)
        {
            try
            {
                // --- 1. LẤY DỮ LIỆU ---
                SalesContractDTO contract = contractBLL.GetContractDetails(contractId);
                if (contract == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin hợp đồng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var items = contractBLL.GetItemsByContract(contractId);
                if (items == null || items.Count == 0)
                {
                    MessageBox.Show("Hợp đồng chưa có nền mẫu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var firstItem = items.FirstOrDefault();

                var paramBLL = new ContractParameterBLL();
                var results = paramBLL.GetFieldLabParametersWithResults(firstItem.ItemId);

                // --- 2. TẠO DOCUMENT ---
                PdfDocument doc = new PdfDocument(PageSize.A4, 40, 30, 30, 120);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));

                // --- 3. TẠO FONT ---
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                BaseFont bf;
                try
                {
                    bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch
                {
                    bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                }

                // --- 4. XỬ LÝ LOGO IN CHÌM (WATERMARK) ---
                PdfImage watermarkImg = null;
                try
                {

                    System.Drawing.Bitmap resourceLogo = null;

                    try { resourceLogo = Properties.Resources.logo; } catch { }

                    if (resourceLogo != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            resourceLogo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            watermarkImg = PdfImage.GetInstance(ms.ToArray());
                        }
                    }
                    else
                    {
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");
                        if (File.Exists(path))
                        {
                            watermarkImg = PdfImage.GetInstance(path);
                        }
                        else
                        {
                            if (Properties.Resources.logo != null)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    Properties.Resources.logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                    watermarkImg = PdfImage.GetInstance(ms.ToArray());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Lỗi load watermark: " + ex.Message);
                }

                writer.PageEvent = new PDFPageFooter(bf, watermarkImg);

                doc.Open();

                // Định nghĩa các Style Font
                PdfFont fTitle = new PdfFont(bf, 14, PdfFont.BOLD);
                PdfFont fHeader = new PdfFont(bf, 11, PdfFont.BOLD);
                PdfFont fNormal = new PdfFont(bf, 11, PdfFont.NORMAL);
                PdfFont fItalic = new PdfFont(bf, 11, PdfFont.ITALIC);
                PdfFont fCompany = new PdfFont(bf, 12, PdfFont.BOLD);

                // HEADER: LOGO VÀ THÔNG TIN CÔNG TY
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 20f, 80f });

                // Cột 1: Logo Header (Góc trái trên)
                PdfPCell cellLogo = new PdfPCell();
                cellLogo.Border = PdfRectangle.NO_BORDER;
                cellLogo.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
                cellLogo.HorizontalAlignment = PdfElement.ALIGN_CENTER;

                if (Properties.Resources.logo != null)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Properties.Resources.logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            PdfImage logoData = PdfImage.GetInstance(ms.ToArray());
                            logoData.ScaleToFit(70f, 70f);
                            cellLogo.AddElement(logoData);
                        }
                    }
                    catch { }
                }
                headerTable.AddCell(cellLogo);

                // Cột 2: Thông tin công ty
                PdfPCell cellInfo = new PdfPCell();
                cellInfo.Border = PdfRectangle.NO_BORDER;
                cellInfo.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
                cellInfo.PaddingLeft = 10f;

                PdfParagraph pCompName = new PdfParagraph("CÔNG TY TNHH GIẢI PHÁP MÔI TRƯỜNG ECOGREEN", fCompany);
                pCompName.Alignment = PdfElement.ALIGN_LEFT;
                cellInfo.AddElement(pCompName);

                PdfParagraph pAddress = new PdfParagraph("Địa chỉ: 19 Nguyễn Hữu Thọ, Phường Tân Phong, Quận 7, Thành Phố Hồ Chí Minh", fNormal);
                cellInfo.AddElement(pAddress);

                PdfParagraph pContact = new PdfParagraph("Điện thoại: 0123456789                  Email: ecogreencompany@gmail.com", fNormal);
                cellInfo.AddElement(pContact);

                headerTable.AddCell(cellInfo);
                doc.Add(headerTable);

                // Đường kẻ đậm dưới Header
                PdfParagraph pLine = new PdfParagraph(new PdfChunk(new iTextSharp.text.pdf.draw.LineSeparator(1.0f, 100f, BaseColor.BLACK, PdfElement.ALIGN_CENTER, -5f)));
                doc.Add(pLine);

                // Tiêu đề Phiếu
                PdfParagraph pDocTitle = new PdfParagraph("PHIẾU KẾT QUẢ THỬ NGHIỆM", fTitle);
                pDocTitle.Alignment = PdfElement.ALIGN_CENTER;
                pDocTitle.SpacingBefore = 15f;
                pDocTitle.SpacingAfter = 10f;
                doc.Add(pDocTitle);

                // PHẦN I: THÔNG TIN CHUNG
                doc.Add(new PdfParagraph("I. THÔNG TIN CHUNG", fHeader));

                PdfPTable tblInfo = new PdfPTable(2);
                tblInfo.WidthPercentage = 100;
                tblInfo.SpacingBefore = 5f;
                tblInfo.SpacingAfter = 10f;
                tblInfo.SetWidths(new float[] { 30f, 70f });

                void AddInfoRow(string label, string content)
                {
                    PdfPCell c1 = new PdfPCell(new PdfPhrase(label, fNormal));
                    c1.Padding = 4f;
                    PdfPCell c2 = new PdfPCell(new PdfPhrase(content, fNormal));
                    c2.Padding = 4f;
                    tblInfo.AddCell(c1);
                    tblInfo.AddCell(c2);
                }

                AddInfoRow("Tên khách hàng", contract.CompanyName ?? "");
                AddInfoRow("Địa chỉ", contract.Address ?? "");
                AddInfoRow("Địa điểm quan trắc", firstItem.SampleLocation ?? "");
                AddInfoRow("Loại mẫu", firstItem.SampleName ?? "");
                AddInfoRow("Vị trí lấy mẫu", $"{firstItem.SampleCode}: {firstItem.SampleName}");
                AddInfoRow("Ngày lấy mẫu", DateTime.Now.ToString("dd/MM/yyyy"));
                AddInfoRow("Thời gian hoàn chỉnh", DateTime.Now.AddDays(5).ToString("dd/MM/yyyy"));

                doc.Add(tblInfo);

                // PHẦN II: KẾT QUẢ
                doc.Add(new PdfParagraph("II. KẾT QUẢ", fHeader));

                PdfPTable tblResult = new PdfPTable(6);
                tblResult.WidthPercentage = 100;
                tblResult.SpacingBefore = 5f;
                tblResult.SpacingAfter = 10f;
                tblResult.SetWidths(new float[] { 6f, 20f, 10f, 25f, 15f, 15f });

                string[] tableHeaders = { "STT", "Chỉ tiêu", "Đơn vị", "Phương pháp thực hiện", "Kết quả", "QCVN" };
                foreach (var h in tableHeaders)
                {
                    PdfPCell cell = new PdfPCell(new PdfPhrase(h, fHeader));
                    cell.HorizontalAlignment = PdfElement.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
                    cell.Padding = 5f;
                    tblResult.AddCell(cell);
                }

                int stt = 1;
                foreach (var item in results)
                {
                    PdfPCell CenterCell(string txt, PdfFont f)
                    {
                        var c = new PdfPCell(new PdfPhrase(txt, f));
                        c.HorizontalAlignment = PdfElement.ALIGN_CENTER;
                        c.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
                        c.Padding = 4f;
                        return c;
                    }
                    PdfPCell LeftCell(string txt, PdfFont f)
                    {
                        var c = new PdfPCell(new PdfPhrase(txt, f));
                        c.Padding = 4f;
                        return c;
                    }

                    tblResult.AddCell(CenterCell(stt.ToString(), fNormal));
                    tblResult.AddCell(LeftCell(item.ParameterName, fNormal));
                    tblResult.AddCell(CenterCell(item.UnitSymbol, fNormal));
                    tblResult.AddCell(LeftCell("SMEWW 3114B:2023", fItalic));
                    tblResult.AddCell(CenterCell(item.Value?.ToString() ?? "", fNormal));

                    string qcvn = "";

                    if (item.MinValue.HasValue || item.MaxValue.HasValue)
                    {


                        string min = item.MinValue.HasValue ? item.MinValue.Value.ToString("0.####################") : "0";
                        string max = item.MaxValue.HasValue ? item.MaxValue.Value.ToString("0.####################") : "";

                        qcvn = $"{min}-{max}";
                    }

                    tblResult.AddCell(CenterCell(qcvn, fNormal));

                    stt++;
                }
                doc.Add(tblResult);

                // PHẦN GHI CHÚ
                PdfParagraph pDate = new PdfParagraph($"Tp Hồ Chí Minh, ngày {DateTime.Now.Day}, tháng {DateTime.Now.Month}, năm {DateTime.Now.Year}", fItalic);
                pDate.Alignment = PdfElement.ALIGN_RIGHT;
                pDate.SpacingBefore = 10f;
                pDate.IndentationRight = 10f;
                doc.Add(pDate);

                PdfPTable tblSign = new PdfPTable(3);
                tblSign.WidthPercentage = 100;
                tblSign.SpacingBefore = 10f;
                tblSign.KeepTogether = true;

                // --- HÀNG 1: TIÊU ĐỀ CHỨC VỤ ---

                // Helper tạo ô Tiêu đề (Căn giữa, không viền)
                PdfPCell CreateTitleCell(string line1, string line2 = "")
                {
                    PdfPCell c = new PdfPCell();
                    c.Border = PdfRectangle.NO_BORDER;
                    c.HorizontalAlignment = PdfElement.ALIGN_CENTER;
                    c.VerticalAlignment = PdfElement.ALIGN_TOP;

                    c.AddElement(new PdfParagraph(line1, fHeader) { Alignment = PdfElement.ALIGN_CENTER });
                    if (!string.IsNullOrEmpty(line2))
                        c.AddElement(new PdfParagraph(line2, fHeader) { Alignment = PdfElement.ALIGN_CENTER });

                    return c;
                }

                tblSign.AddCell(CreateTitleCell("NGƯỜI LẬP"));
                tblSign.AddCell(CreateTitleCell("TRƯỞNG PHÒNG", "PHÂN TÍCH"));
                tblSign.AddCell(CreateTitleCell("ĐẠI DIỆN CÔNG TY", "TỔNG GIÁM ĐỐC"));

                // --- HÀNG 2: TÊN NGƯỜI KÝ ---

                // Helper tạo ô Tên (Có PaddingTop để tạo khoảng trống ký tên)
                PdfPCell CreateNameCell(string name, bool isBold = false)
                {
                    PdfPCell c = new PdfPCell();
                    c.Border = PdfRectangle.NO_BORDER;
                    c.HorizontalAlignment = PdfElement.ALIGN_CENTER;
                    c.VerticalAlignment = PdfElement.ALIGN_TOP;

                    c.PaddingTop = 60f; // Khoảng cách để ký 

                    c.AddElement(new PdfParagraph(name, isBold ? fHeader : fNormal) { Alignment = PdfElement.ALIGN_CENTER });
                    return c;
                }

                tblSign.AddCell(CreateNameCell("Trần Văn A", true));
                tblSign.AddCell(CreateNameCell("Nguyễn Thị B", true));
                tblSign.AddCell(CreateNameCell("Nguyễn Văn C", true));

                doc.Add(tblSign);

                doc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tạo nhiều file PDF, mỗi file cho một nền mẫu
        private List<string> CreateContractPdfs(string contractId)
        {
            List<string> pdfPaths = new List<string>();
            try
            {
                SalesContractDTO contract = contractBLL.GetContractDetails(contractId);
                if (contract == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin hợp đồng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return pdfPaths;
                }

                var items = contractBLL.GetItemsByContract(contractId);
                if (items == null || items.Count == 0)
                {
                    MessageBox.Show("Hợp đồng chưa có nền mẫu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return pdfPaths;
                }

                string exportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);

                var paramBLL = new ContractParameterBLL();
                var userBLL = new UserBLL();
                var (planningHeadName, labHeadName, salesHeadName) = GetSignatureNames(userBLL);

                foreach (var item in items)
                {
                    var results = paramBLL.GetFieldLabParametersWithResults(item.ItemId);
                    if (results == null || results.Count == 0)
                    {
                        continue; // Bỏ qua nền mẫu không có kết quả
                    }

                    var measurementInfo = paramBLL.GetLatestBatchInfo(item.ItemId);
                    string safeItemId = SanitizeFileName(item.ItemId);
                    string pdfPath = Path.Combine(exportDir, $"BaoCao_{contractId}_{safeItemId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                    
                    CreateContractPdfForItem(pdfPath, contract, item, results, measurementInfo, planningHeadName, labHeadName, salesHeadName);
                    
                    if (File.Exists(pdfPath))
                    {
                        pdfPaths.Add(pdfPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return pdfPaths;
        }

        // Tạo PDF cho một nền mẫu cụ thể
        private void CreateContractPdfForItem(string outputPath, SalesContractDTO contract, ContractItemDTO item, List<ParameterResultDTO> results, MeasurementInfoDTO measurementInfo, string planningHeadName, string labHeadName, string salesHeadName)
        {
            try
            {
                // --- 2. TẠO DOCUMENT ---
                PdfDocument doc = new PdfDocument(PageSize.A4, 40, 30, 30, 120);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));

                // --- 3. TẠO FONT ---
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                BaseFont bf;
                try
                {
                    bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch
                {
                    bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                }

                // --- 4. XỬ LÝ LOGO IN CHÌM (WATERMARK) ---
                PdfImage watermarkImg = null;
                try
                {
                    System.Drawing.Bitmap resourceLogo = null;
                    try { resourceLogo = Properties.Resources.logo; } catch { }

                    if (resourceLogo != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            resourceLogo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            watermarkImg = PdfImage.GetInstance(ms.ToArray());
                        }
                    }
                    else
                    {
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");
                        if (File.Exists(path))
                        {
                            watermarkImg = PdfImage.GetInstance(path);
                        }
                        else
                        {
                            if (Properties.Resources.logo != null)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    Properties.Resources.logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                    watermarkImg = PdfImage.GetInstance(ms.ToArray());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Lỗi load watermark: " + ex.Message);
                }

                writer.PageEvent = new PDFPageFooter(bf, watermarkImg);
                doc.Open();

                // Định nghĩa các Style Font
                PdfFont fTitle = new PdfFont(bf, 14, PdfFont.BOLD);
                PdfFont fHeader = new PdfFont(bf, 11, PdfFont.BOLD);
                PdfFont fNormal = new PdfFont(bf, 11, PdfFont.NORMAL);
                PdfFont fItalic = new PdfFont(bf, 11, PdfFont.ITALIC);
                PdfFont fCompany = new PdfFont(bf, 12, PdfFont.BOLD);

                // HEADER: LOGO VÀ THÔNG TIN CÔNG TY
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 20f, 80f });

                // Cột 1: Logo Header
                PdfPCell cellLogo = new PdfPCell();
                cellLogo.Border = PdfRectangle.NO_BORDER;
                cellLogo.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
                cellLogo.HorizontalAlignment = PdfElement.ALIGN_CENTER;

                if (Properties.Resources.logo != null)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Properties.Resources.logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            PdfImage logoData = PdfImage.GetInstance(ms.ToArray());
                            logoData.ScaleToFit(70f, 70f);
                            cellLogo.AddElement(logoData);
                        }
                    }
                    catch { }
                }
                headerTable.AddCell(cellLogo);

                // Cột 2: Thông tin công ty
                PdfPCell cellInfo = new PdfPCell();
                cellInfo.Border = PdfRectangle.NO_BORDER;
                cellInfo.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
                cellInfo.PaddingLeft = 10f;

                PdfParagraph pCompName = new PdfParagraph("CÔNG TY TNHH GIẢI PHÁP MÔI TRƯỜNG ECOGREEN", fCompany);
                pCompName.Alignment = PdfElement.ALIGN_LEFT;
                cellInfo.AddElement(pCompName);

                PdfParagraph pAddress = new PdfParagraph("Địa chỉ: 19 Nguyễn Hữu Thọ, Phường Tân Phong, Quận 7, Thành Phố Hồ Chí Minh", fNormal);
                cellInfo.AddElement(pAddress);

                PdfParagraph pContact = new PdfParagraph("Điện thoại: 0123456789                  Email: ecogreencompany@gmail.com", fNormal);
                cellInfo.AddElement(pContact);

                headerTable.AddCell(cellInfo);
                doc.Add(headerTable);

                // Đường kẻ đậm dưới Header
                PdfParagraph pLine = new PdfParagraph(new PdfChunk(new iTextSharp.text.pdf.draw.LineSeparator(1.0f, 100f, BaseColor.BLACK, PdfElement.ALIGN_CENTER, -5f)));
                doc.Add(pLine);

                // Tiêu đề Phiếu
                PdfParagraph pDocTitle = new PdfParagraph("PHIẾU KẾT QUẢ THỬ NGHIỆM", fTitle);
                pDocTitle.Alignment = PdfElement.ALIGN_CENTER;
                pDocTitle.SpacingBefore = 15f;
                pDocTitle.SpacingAfter = 10f;
                doc.Add(pDocTitle);

                // PHẦN I: THÔNG TIN CHUNG
                doc.Add(new PdfParagraph("I. THÔNG TIN CHUNG", fHeader));

                PdfPTable tblInfo = new PdfPTable(2);
                tblInfo.WidthPercentage = 100;
                tblInfo.SpacingBefore = 5f;
                tblInfo.SpacingAfter = 10f;
                tblInfo.SetWidths(new float[] { 30f, 70f });

                void AddInfoRow(string label, string content)
                {
                    PdfPCell c1 = new PdfPCell(new PdfPhrase(label, fNormal));
                    c1.Padding = 4f;
                    PdfPCell c2 = new PdfPCell(new PdfPhrase(content, fNormal));
                    c2.Padding = 4f;
                    tblInfo.AddCell(c1);
                    tblInfo.AddCell(c2);
                }

                AddInfoRow("Tên khách hàng", contract.CompanyName ?? "");
                AddInfoRow("Địa chỉ", contract.Address ?? "");
                AddInfoRow("Địa điểm quan trắc", item.SampleLocation ?? "");
                AddInfoRow("Loại mẫu", item.SampleName ?? "");
                AddInfoRow("Vị trí lấy mẫu", $"{item.SampleCode}: {item.SampleName}");
                AddInfoRow("Ngày lấy mẫu", FormatDate(measurementInfo?.StartDate));
                AddInfoRow("Thời gian hoàn chỉnh", FormatDate(measurementInfo?.LastResultUpdatedAt));

                doc.Add(tblInfo);

                // PHẦN II: KẾT QUẢ
                doc.Add(new PdfParagraph("II. KẾT QUẢ", fHeader));

                PdfPTable tblResult = new PdfPTable(7); // Thêm cột Ghi chú
                tblResult.WidthPercentage = 100;
                tblResult.SpacingBefore = 5f;
                tblResult.SpacingAfter = 10f;
                tblResult.SetWidths(new float[] { 5f, 18f, 8f, 20f, 12f, 12f, 15f });

                string[] tableHeaders = { "STT", "Chỉ tiêu", "Đơn vị", "Phương pháp thực hiện", "Kết quả", "QCVN", "Ghi chú" };
                foreach (var h in tableHeaders)
                {
                    PdfPCell cell = new PdfPCell(new PdfPhrase(h, fHeader));
                    cell.HorizontalAlignment = PdfElement.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
                    cell.Padding = 5f;
                    tblResult.AddCell(cell);
                }

                int stt = 1;
                foreach (var result in results)
                {
                    PdfPCell CenterCell(string txt, PdfFont f)
                    {
                        var c = new PdfPCell(new PdfPhrase(txt, f));
                        c.HorizontalAlignment = PdfElement.ALIGN_CENTER;
                        c.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
                        c.Padding = 4f;
                        return c;
                    }
                    PdfPCell LeftCell(string txt, PdfFont f)
                    {
                        var c = new PdfPCell(new PdfPhrase(txt, f));
                        c.Padding = 4f;
                        return c;
                    }

                    tblResult.AddCell(CenterCell(stt.ToString(), fNormal));
                    tblResult.AddCell(LeftCell(result.ParameterName, fNormal));
                    tblResult.AddCell(CenterCell(result.UnitSymbol, fNormal));
                    tblResult.AddCell(LeftCell("SMEWW 3114B:2023", fItalic));
                    tblResult.AddCell(CenterCell(FormatResultValue(result.Value), fNormal));
                    tblResult.AddCell(CenterCell(FormatLimitRange(result.MinValue, result.MaxValue), fNormal));
                    tblResult.AddCell(LeftCell(result.Note ?? "", fNormal));

                    stt++;
                }
                doc.Add(tblResult);

                // ========== PHẦN CHỮ KÝ ==========
                PdfParagraph dateLine = new PdfParagraph($"Tp Hồ Chí Minh, ngày {DateTime.Now.Day}, tháng {DateTime.Now.Month}, năm {DateTime.Now.Year}", fNormal);
                dateLine.Alignment = PdfElement.ALIGN_RIGHT;
                dateLine.SpacingBefore = 20;
                dateLine.SpacingAfter = 30;
                doc.Add(dateLine);

                // Bảng chữ ký
                PdfPTable signatureTable = new PdfPTable(3);
                signatureTable.WidthPercentage = 100;
                signatureTable.SetWidths(new float[] { 33f, 34f, 33f });
                signatureTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                AddSignatureCell(signatureTable, "NGƯỜI LẬP", null, planningHeadName, fHeader, fNormal);
                AddSignatureCell(signatureTable, "TRƯỞNG PHÒNG PHÂN TÍCH", null, labHeadName, fHeader, fNormal);
                AddSignatureCell(signatureTable, "ĐẠI DIỆN CÔNG TY", "TỔNG GIÁM ĐỐC", salesHeadName, fHeader, fNormal);

                doc.Add(signatureTable);

                doc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper methods
        private string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return "Unknown";
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        private string FormatDate(DateTime? date)
        {
            if (!date.HasValue) return DateTime.Now.ToString("dd/MM/yyyy");
            return date.Value.ToString("dd/MM/yyyy");
        }

        private string FormatResultValue(decimal? value)
        {
            if (!value.HasValue) return "";
            return value.Value.ToString();
        }

        private string FormatLimitRange(decimal? minValue, decimal? maxValue)
        {
            if (!minValue.HasValue && !maxValue.HasValue) return "";
            
            string min = minValue.HasValue ? FormatLimitValue(minValue.Value) : "0";
            string max = maxValue.HasValue ? FormatLimitValue(maxValue.Value) : "";
            
            return string.IsNullOrEmpty(max) ? min : $"{min}-{max}";
        }

        private string FormatLimitValue(decimal value)
        {
            // Format với tối đa 2 số thập phân, trừ khi số rất nhỏ thì giữ nhiều hơn
            if (Math.Abs(value) < 0.01m && value != 0)
            {
                // Số rất nhỏ, giữ nhiều số thập phân hơn
                return value.ToString("0.####################");
            }
            // Số bình thường, chỉ 2 số thập phân
            return value.ToString("0.##");
        }

        private (string planning, string lab, string sales) GetSignatureNames(UserBLL userBLL)
        {
            string GetHeadName(string departmentId, string overrideKey, string fallbackName)
            {
                string overrideName = ConfigurationManager.AppSettings[overrideKey];
                if (!string.IsNullOrWhiteSpace(overrideName))
                {
                    return overrideName.Trim();
                }

                var head = userBLL.GetDepartmentHead(departmentId);
                if (head != null && !string.IsNullOrWhiteSpace(head.FullName))
                {
                    if (!IsPlaceholderName(head.FullName))
                    {
                        return head.FullName.Trim();
                    }
                }
                return fallbackName;
            }

            string PlanningDepartmentId = "D003";
            string LabDepartmentId = "D005";
            string SalesDepartmentId = "D002";

            string planningHeadName = GetHeadName(PlanningDepartmentId, "PlanningHeadNameOverride", "Chưa cập nhật");
            string labHeadName = GetHeadName(LabDepartmentId, "LabHeadNameOverride", "Chưa cập nhật");
            string salesHeadName = GetHeadName(SalesDepartmentId, "SalesHeadNameOverride", "Chưa cập nhật");

            return (planningHeadName, labHeadName, salesHeadName);
        }

        private bool IsPlaceholderName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return true;
            string normalizedName = name.Trim().ToLowerInvariant();
            return normalizedName.Contains("trưởng phòng") ||
                   normalizedName.Contains("admin") ||
                   normalizedName.Contains("chưa cập nhật");
        }

        private void AddSignatureCell(PdfPTable table, string title, string subtitle, string name, PdfFont titleFont, PdfFont nameFont)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.HorizontalAlignment = PdfElement.ALIGN_CENTER;
            cell.VerticalAlignment = PdfElement.ALIGN_TOP;
            cell.PaddingTop = 5;
            cell.PaddingBottom = 5;
            cell.MinimumHeight = 120f;

            cell.AddElement(new PdfParagraph(title, titleFont) { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 2 });
            if (!string.IsNullOrEmpty(subtitle))
            {
                cell.AddElement(new PdfParagraph(subtitle, titleFont) { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 2 });
            }
            cell.AddElement(new PdfParagraph(" ", nameFont) { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 30 });
            cell.AddElement(new PdfParagraph(name, nameFont) { Alignment = PdfElement.ALIGN_CENTER });

            table.AddCell(cell);
        }

        // Class xử lý Footer tự động cho mọi trang
        public class PDFPageFooter : PdfPageEventHelper
        {
            private BaseFont _baseFont;
            private PdfImage _watermarkLogo;

            public PDFPageFooter(BaseFont baseFont, PdfImage logo = null)
            {
                _baseFont = baseFont;
                _watermarkLogo = logo;
            }

            public override void OnEndPage(PdfWriter writer, PdfDocument document)
            {
                // PHẦN 1: WATERMARK 
                if (_watermarkLogo != null)
                {
                    PdfContentByte under = writer.DirectContentUnder;
                    under.SaveState();
                    PdfGState gs = new PdfGState();
                    gs.FillOpacity = 0.15f;
                    under.SetGState(gs);
                    float pageWidth = document.PageSize.Width;
                    float pageHeight = document.PageSize.Height;
                    float watermarkWidth = 400f;
                    float watermarkHeight = 400f * (_watermarkLogo.Height / _watermarkLogo.Width);
                    float x = (pageWidth - watermarkWidth) / 2;
                    float y = (pageHeight - watermarkHeight) / 2;
                    PdfImage img = PdfImage.GetInstance(_watermarkLogo);
                    img.ScaleAbsolute(watermarkWidth, watermarkHeight);
                    img.SetAbsolutePosition(x, y);
                    under.AddImage(img);
                    under.RestoreState();
                }

                // --- PHẦN 2: VẼ FOOTER (SỬA LẠI TỌA ĐỘ) ---
                PdfContentByte cb = writer.DirectContent;
                var fontFooter = new PdfFont(_baseFont, 10, PdfFont.ITALIC);
                var fontSmall = new PdfFont(_baseFont, 10, PdfFont.NORMAL);

                float footerY = 70f;

                // 1. Vẽ đường kẻ ngang
                cb.SaveState();
                cb.SetLineWidth(0.5f);
                cb.MoveTo(document.Left, footerY);
                cb.LineTo(document.Right, footerY);
                cb.Stroke();
                cb.RestoreState();

                // 2. Vẽ các dòng Disclaimer 
                ColumnText ct = new ColumnText(cb);
                // Vẽ trong khoảng từ Y=70 đến Y=120
                ct.SetSimpleColumn(document.Left, footerY, document.Right, footerY + 50, 10, PdfElement.ALIGN_LEFT);

                PdfParagraph pDisclaimer = new PdfParagraph();
                pDisclaimer.SetLeading(11f, 0f); 
                pDisclaimer.Add(new PdfChunk("- Kết quả chỉ có giá trị phân tích.\n", fontSmall));
                pDisclaimer.Add(new PdfChunk("- Các yêu cầu và tên khách hàng được ghi theo yêu cầu khách hàng.\n", fontSmall));
                pDisclaimer.Add(new PdfChunk("- Không sao lưu khi chưa được cơ quan thẩm quyền cho phép.", fontSmall));

                ct.AddElement(pDisclaimer);
                ct.Go();

                // 3. Vẽ dòng QCVN và Số trang (Sát đáy trang, khoảng Y=30)
                PdfPTable footerTbl = new PdfPTable(3);
                footerTbl.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                footerTbl.HorizontalAlignment = PdfElement.ALIGN_CENTER;
                footerTbl.SetWidths(new float[] { 40f, 30f, 30f });

                PdfPCell cell1 = new PdfPCell(new PdfPhrase("QCVN 03:2024/BTNMT", fontFooter));
                cell1.Border = PdfRectangle.NO_BORDER;
                cell1.HorizontalAlignment = PdfElement.ALIGN_LEFT;

                PdfPCell cell2 = new PdfPCell(new PdfPhrase("Ban hành lần: 01", fontFooter));
                cell2.Border = PdfRectangle.NO_BORDER;
                cell2.HorizontalAlignment = PdfElement.ALIGN_CENTER;

                PdfPCell cell3 = new PdfPCell(new PdfPhrase($"Trang: {writer.PageNumber}", fontFooter));
                cell3.Border = PdfRectangle.NO_BORDER;
                cell3.HorizontalAlignment = PdfElement.ALIGN_RIGHT;

                footerTbl.AddCell(cell1);
                footerTbl.AddCell(cell2);
                footerTbl.AddCell(cell3);

                // Vẽ bảng tại vị trí Y = 30 (Sát đáy)
                footerTbl.WriteSelectedRows(0, -1, document.LeftMargin, 30, cb);
            }
        }


        private void AddInfoRow(PdfPTable table, string label, string value, PdfFont font)
        {
            PdfPCell labelCell = new PdfPCell(new PdfPhrase(label, font));
            labelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            labelCell.Padding = 5;
            table.AddCell(labelCell);

            PdfPCell valueCell = new PdfPCell(new PdfPhrase(value, font));
            valueCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            valueCell.Padding = 5;
            table.AddCell(valueCell);
        }

        private void AddTableHeaderCell(PdfPTable table, string text, PdfFont font)
        {
            PdfPCell cell = new PdfPCell(new PdfPhrase(text, font));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = PdfElement.ALIGN_CENTER;
            cell.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private void AddTableCell(PdfPTable table, string text, PdfFont font)
        {
            PdfPCell cell = new PdfPCell(new PdfPhrase(text, font));
            cell.HorizontalAlignment = PdfElement.ALIGN_CENTER;
            cell.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private void AddSignatureCell(PdfPTable table, string title, string name, PdfFont titleFont, PdfFont nameFont)
        {
            PdfPCell cell = new PdfPCell();
            cell.AddElement(new PdfParagraph(title, titleFont));
            cell.AddElement(new PdfParagraph(name, nameFont));
            cell.HorizontalAlignment = PdfElement.ALIGN_CENTER;
            cell.VerticalAlignment = PdfElement.ALIGN_MIDDLE;
            cell.PaddingTop = 20;
            table.AddCell(cell);
        }

        private string Val(DataGridViewRow row, string col)
        {
            return Convert.ToString(row.Cells[col]?.Value ?? "");
        }

        private string Sanitize(string text)
        {
            return EscapePdfText(RemoveDiacritics(text ?? string.Empty));
        }

        private void WriteBasicPdf(string outputPath, List<string> lines)
        {
            var sb = new StringBuilder();
            int y = 770;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) { y -= 18; continue; }
                if (line.StartsWith("# "))
                {
                    string t = EscapePdfText(RemoveDiacritics(line.Substring(2)));
                    sb.Append($"BT /F1 24 Tf 72 {y} Td ({t}) Tj ET\n");
                    y -= 26;
                }
                else
                {
                    string t = line; // đã sanitize trước
                    sb.Append($"BT /F1 12 Tf 72 {y} Td ({t}) Tj ET\n");
                    y -= 16;
                }
                if (y < 60) break; // tránh vượt trang (đơn trang)
            }

            string content = sb.ToString();
            int len = Encoding.ASCII.GetByteCount(content);

            var header = "%PDF-1.4\n";
            var obj1 = "1 0 obj<</Type/Catalog/Pages 2 0 R>>endobj\n";
            var obj2 = "2 0 obj<</Type/Pages/Count 1/Kids[3 0 R]>>endobj\n";
            var obj3 = "3 0 obj<</Type/Page/Parent 2 0 R/MediaBox[0 0 595 842]/Contents 4 0 R/Resources<</Font<</F1 5 0 R>>>>>>endobj\n";
            var obj4 = $"4 0 obj<</Length {len}>>stream\n{content}endstream endobj\n";
            var obj5 = "5 0 obj<</Type/Font/Subtype/Type1/BaseFont/Helvetica>>endobj\n";

            var parts = new List<string> { obj1, obj2, obj3, obj4, obj5 };
            var builder = new StringBuilder();
            builder.Append(header);
            var offsets = new List<int>();
            int pos = Encoding.ASCII.GetByteCount(header);
            foreach (var p in parts)
            {
                offsets.Add(pos);
                builder.Append(p);
                pos += Encoding.ASCII.GetByteCount(p);
            }

            int xrefPos = pos;
            var xref = new StringBuilder();
            xref.Append("xref\n");
            xref.Append("0 6\n");
            xref.Append("0000000000 65535 f \n");
            foreach (var off in offsets)
            {
                xref.Append(off.ToString("0000000000"));
                xref.Append(" 00000 n \n");
            }
            builder.Append(xref.ToString());
            builder.Append("trailer<</Size 6/Root 1 0 R>>\n");
            builder.Append($"startxref\n{xrefPos}\n%%EOF");

            File.WriteAllText(outputPath, builder.ToString(), Encoding.ASCII);
        }

        private string EscapePdfText(string text)
        {
            return text.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            string normalized = text.Normalize(NormalizationForm.FormD);
            var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            string noMarks = new string(chars).Normalize(NormalizationForm.FormC);
            noMarks = noMarks.Replace("Đ", "D").Replace("đ", "d");
            return new string(noMarks.Where(c => c <= 126).ToArray());
        }

        private bool IsValidCompanyNameNoDigits(string name)
        {
            // Cho phép chữ cái (kể cả dấu), khoảng trắng và các ký tự & , . - ( ) / \
            // Không cho phép chữ số
            foreach (var ch in name)
            {
                if (char.IsDigit(ch)) return false;
            }
            return true;
        }

        private bool _isUpdatingExpectedDate = false; // Flag để tránh vòng lặp vô hạn

        private void DtpSigningDate_ValueChanged(object sender, EventArgs e)
        {
            // Tránh vòng lặp vô hạn khi cập nhật dtpExpectedResultDate
            if (_isUpdatingExpectedDate) return;
            
            // Tự động cập nhật ngày dự kiến trả kết quả = ngày ký + 20 ngày
            var signDate = dtpSigningDate.Value.Date;
            _isUpdatingExpectedDate = true;
            try
            {
                dtpExpectedResultDate.Value = signDate.AddDays(20);
            }
            finally
            {
                _isUpdatingExpectedDate = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedContractId))
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation tên doanh nghiệp: bắt buộc nhập
            if (!ContractInputValidator.IsRequiredFieldFilled(txtCompanyName.Text))
            {
                MessageBox.Show("Để trống trường bắt buộc (Tên doanh nghiệp).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCompanyName.Focus();
                return;
            }

            // Validation tên doanh nghiệp: không chứa số
            if (!IsValidCompanyNameNoDigits(txtCompanyName.Text))
            {
                MessageBox.Show("Tên doanh nghiệp không được chứa số.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCompanyName.Focus();
                txtCompanyName.SelectAll();
                return;
            }

            // Validation tên người đại diện: không chứa số
            if (!string.IsNullOrWhiteSpace(txtRepresentativeName.Text) && !IsValidCompanyNameNoDigits(txtRepresentativeName.Text))
            {
                MessageBox.Show("Tên người đại diện không được chứa số.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRepresentativeName.Focus();
                txtRepresentativeName.SelectAll();
                return;
            }

            // Validation cho số điện thoại
            if (!string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                if (!ContractInputValidator.IsValidPhoneNumber(txtPhoneNumber.Text))
                {
                    MessageBox.Show("Số điện thoại phải có 10 chữ số và bắt đầu từ số 0.\nVí dụ: 0123456789", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhoneNumber.Focus();
                    txtPhoneNumber.SelectAll();
                    return;
                }
            }

            // Validation ngày trả kết quả >= ngày ký
            if (!ContractInputValidator.IsResultDateValid(dtpSigningDate.Value, dtpExpectedResultDate.Value))
            {
                MessageBox.Show("Ngày trả kết quả phải lớn hơn hoặc bằng ngày ký hợp đồng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpExpectedResultDate.Focus();
                return;
            }

            // Validation cho email
            if (!ContractInputValidator.IsValidEmail(txtRepresentativeEmail.Text))
            {
                MessageBox.Show("Email không đúng định dạng.\nVí dụ: example@domain.com", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRepresentativeEmail.Focus();
                txtRepresentativeEmail.SelectAll();
                return;
            }

            SalesContractDTO updatedContract = new SalesContractDTO
            {
                ContractID = selectedContractId,
                CustomerID = txtCustomerID.Text,
                CompanyName = txtCompanyName.Text,
                Address = txtAddress.Text,
                RepresentativeName = txtRepresentativeName.Text,
                RepresentativeEmail = txtRepresentativeEmail.Text,
                PhoneNumber = txtPhoneNumber.Text,
                SigningDate = dtpSigningDate.Value,
                ExpectedResultDate = dtpExpectedResultDate.Value
            };

            bool success = contractBLL.UpdateContract(updatedContract);

            if (success)
            {
                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fullContractList = contractBLL.GetContractSummaryList();
                txtSearch_TextChanged(txtSearch, EventArgs.Empty);
                // Phát sự kiện để các khu vực khác cập nhật real-time
                GlobalEventBus.RaiseContractDataChanged();
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCompanyName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}