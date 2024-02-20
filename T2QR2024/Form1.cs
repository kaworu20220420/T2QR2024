using QRCoder;
using System.Text;
using Ude;

namespace T2QR2024
{
	public partial class Form1 : Form
	{
		private List<string> fileContents = new List<string>();
		private int currentIndex = 0;
		private const int MaxQRCodeByteSize = 1663; // QR�R�[�h�̍ő�o�C�g�T�C�Y
		private ToolTip toolTip = new ToolTip();

		public Form1()
		{
			InitializeComponent();
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			fileContents.Clear();
			foreach (string file in files)
			{
				// �t�@�C������e�L�X�g��ǂݍ���
				string text;
				using (var fs = File.OpenRead(file))
				{
					var detector = new CharsetDetector();
					detector.Feed(fs);
					detector.DataEnd();
					if (detector.Charset != null)
					{
						var encoding = Encoding.GetEncoding(detector.Charset);
						text = File.ReadAllText(file, encoding);
					}
					else
					{
						// �f�t�H���g�̃G���R�[�f�B���O���g�p
						text = File.ReadAllText(file);
					}
				}

				// ���s�A�^�u�A��_�A�R���}�ŕ���
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
			currentIndex = 0;
			DisplayQRCode();
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
					if (trimmedPart.Length > 0)
					{
						var encodedPart = Encoding.UTF8.GetBytes(trimmedPart);
						if (encodedPart.Length <= MaxQRCodeByteSize)
						{
							newParts.Add(trimmedPart);
						}
						else
						{
							// �������K�v�ȏꍇ
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
							if (sb.Length > 0)
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

				// PictureBox�̃T�C�Y�ɍ��킹��QR�R�[�h�̃C���[�W���k��
				MainPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
				MainPictureBox.Image = new Bitmap(qrCodeImage, MainPictureBox.Size);
			}
			else
			{
				MessageBox.Show(text, "The text is too long to be encoded into a single QR code." + text.Length);
			}

			// Form�̃^�C�g���Ɍ��݂̕\�������ƕ�����Ԃ�\��
			this.Text = $"{currentIndex + 1}/{fileContents.Count}";
		}
	}
}
