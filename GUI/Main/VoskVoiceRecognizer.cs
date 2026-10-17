using NAudio.Wave;
using Newtonsoft.Json.Linq; 
using System;
using System.IO;
using System.Windows.Forms;
using Vosk;

namespace GUI
{
    public class VoskVoiceRecognizer : IDisposable
    {
        private WaveInEvent waveIn;
        private VoskRecognizer recognizer;
        private Model model;

        public event EventHandler<string> SpeechRecognized;

        public VoskVoiceRecognizer(string modelPath)
        {
            if (!Directory.Exists(modelPath))
            {
                throw new DirectoryNotFoundException($"Thư mục mô hình không tồn tại: {modelPath}. " +
                    "'.");
            }

            try
            {
                // Kiểm tra kiến trúc tiến trình và gợi ý cài đúng gói Vosk (x64)
                if (IntPtr.Size != 8)
                {
                    throw new InvalidOperationException("Vosk cần chạy kiến trúc x64. Hãy build/running cấu hình x64.");
                }

                Vosk.Vosk.SetLogLevel(-1); // giảm noisy log
                model = new Model(modelPath);
                recognizer = new VoskRecognizer(model, 16000.0f);

                waveIn = new WaveInEvent();
                waveIn.DeviceNumber = 0;
                waveIn.WaveFormat = new WaveFormat(16000, 1);
                waveIn.DataAvailable += WaveIn_DataAvailable;
            }
            catch (DllNotFoundException ex)
            {
                throw new InvalidOperationException("Thiếu thư viện native của Vosk (libvosk.dll). Hãy đảm bảo đã restore NuGet và chạy x64.", ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new InvalidOperationException("Sai kiến trúc libvosk.dll (x86/x64). Hãy chạy x64 và đảm bảo libvosk.dll là win-x64.", ex);
            }
            catch (AccessViolationException ex)
            {
                throw new InvalidOperationException($"Lỗi khởi tạo Vosk: {ex.Message}. " +
                    "", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi khởi tạo bộ nhận dạng giọng nói: {ex.Message}", ex);
            }
        }

        public void Start()
        {
            waveIn.StartRecording();
        }

        public void Stop()
        {
            waveIn.StopRecording();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
            {
                string resultText = GetTextFromResult(recognizer.Result());
                OnSpeechRecognized(resultText);
            }
        }


        private string GetTextFromResult(string jsonResult)
        {
            if (string.IsNullOrWhiteSpace(jsonResult))
            {
                return string.Empty;
            }

            try
            {
                var jsonObject = JObject.Parse(jsonResult);
                return jsonObject["text"]?.ToString() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        protected virtual void OnSpeechRecognized(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                SpeechRecognized?.Invoke(this, text);
            }
        }

        public void Dispose()
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }
            if (recognizer != null)
            {
                recognizer.Dispose();
                recognizer = null;
            }
            if (model != null)
            {
                model.Dispose();
                model = null;
            }
        }
    }
}