using QRCoder;
using System.Text;
using Ude;

namespace T2QR2024
{
	public partial class Form1 : Form
	{
		private List<string> fileContents = new List<string>();
		private int currentIndex = 0;
		private const int MaxQRCodeByteSize = 1663; // QRコードの最大バイトサイズ
		private ToolTip toolTip = new ToolTip();

		public Form1()
		{
			InitializeComponent();
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		private string ReadTextWithEncodingDetection(string path, Stream stream)
		{
			string text;
			var detector = new CharsetDetector();
			detector.Feed(stream);
			detector.DataEnd();
			if (detector.Charset == null || detector.Charset.ToLower() == "utf-8")
			{
				// デフォルトのエンコーディングを使用
				text = path != null ? File.ReadAllText(path) : Encoding.UTF8.GetString(((MemoryStream)stream).ToArray());
			}
			else
			{
				var encoding = Encoding.GetEncoding(detector.Charset);
				text = path != null ? File.ReadAllText(path, encoding) : encoding.GetString(((MemoryStream)stream).ToArray());
			}
			return text;
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text))
			{
				string text = (string)e.Data.GetData(DataFormats.Text);

				// テキストをメモリストリームに変換
				var bytes = Encoding.UTF8.GetBytes(text);
				using (var ms = new MemoryStream(bytes))
				{
					text = ReadTextWithEncodingDetection(null, ms);
				}

				ProcessText(text);
			}
			else if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				fileContents.Clear();
				foreach (string file in files)
				{
					// ファイルからテキストを読み込む
					string text;
					using (var fs = File.OpenRead(file))
					{
						text = ReadTextWithEncodingDetection(file, fs);
					}

					ProcessText(text);
				}
			}
			currentIndex = 0;
			DisplayQRCode();
		}

		private void ProcessText(string text)
		{
			// 改行、タブ、句点、コンマで分割
			var delimiters = new string[] { "\r\n", "\r", "\n" };

			if (text.Length <= MaxQRCodeByteSize)
			{
				fileContents.Add(text);
			}
			else
			{
				var parts = new List<string> { text };
				foreach (var delimiter in delimiters)
				{
					parts = SplitParts(parts, delimiter);
				}
				fileContents.AddRange(parts);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			// Ctrl+Vを押したとき
			if (keyData == (Keys.Control | Keys.V))
			{
				string clipboardText = Clipboard.GetText();
				ProcessText(clipboardText);
				currentIndex = 0;
				DisplayQRCode();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}


		private List<string> SplitParts(List<string> parts, string delimiter)
		{
			var newParts = new List<string>();
			foreach (var part in parts)
			{
				var splitParts = part.Split(delimiter);
				foreach (var splitPart in splitParts)
				{
					var trimmedPart = splitPart.Trim();
					if (0 < trimmedPart.Length)
					{
						var encodedPart = Encoding.UTF8.GetBytes(trimmedPart);
						if (encodedPart.Length <= MaxQRCodeByteSize)
						{
							newParts.Add(trimmedPart);
						}
						else
						{
							// 分割が必要な場合
							var sb = new StringBuilder();
							foreach (var ch in trimmedPart)
							{
								if (Encoding.UTF8.GetByteCount(sb.ToString() + ch) > MaxQRCodeByteSize)
								{
									newParts.Add(sb.ToString());
									sb.Clear();
								}
								sb.Append(ch);
							}
							if (0 < sb.Length)
							{
								newParts.Add(sb.ToString());
							}
						}
					}
				}
			}
			return newParts;
		}

		private void MainPictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (fileContents.Count == 0)
			{
				return;
			}
			currentIndex = (currentIndex + 1) % fileContents.Count;
			DisplayQRCode();
		}

		private void MainPictureBox_MouseEnter(object sender, EventArgs e)
		{
			if (fileContents.Count == 0)
			{
				return;
			}
			toolTip.SetToolTip(MainPictureBox, fileContents[currentIndex]);
		}

		private void DisplayQRCode()
		{
			string text = fileContents[currentIndex];
			if (Encoding.UTF8.GetByteCount(text) <= MaxQRCodeByteSize)
			{
				QRCodeGenerator qrGenerator = new QRCodeGenerator();
				QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
				QRCode qrCode = new QRCode(qrCodeData);
				Bitmap qrCodeImage = qrCode.GetGraphic(20);

				// PictureBoxのサイズに合わせてQRコードのイメージを縮小
				MainPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
				MainPictureBox.Image = new Bitmap(qrCodeImage, MainPictureBox.Size);
			}
			else
			{
				MessageBox.Show(text, "The text is too long to be encoded into a single QR code." + text.Length);
			}

			// Formのタイトルに現在の表示枚数と分割状態を表示
			this.Text = $"{currentIndex + 1}/{fileContents.Count}";
		}

		private void QrButton_Click(object sender, EventArgs e) => QrImagePanel.Visible = true;

		private void OKButton_Click(object sender, EventArgs e) => QrImagePanel.Visible = false;
	}
}
