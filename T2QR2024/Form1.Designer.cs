namespace T2QR2024
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			MainPictureBox = new PictureBox();
			ReadButton = new Button();
			ReadResult = new Label();
			((System.ComponentModel.ISupportInitialize)MainPictureBox).BeginInit();
			SuspendLayout();
			// 
			// MainPictureBox
			// 
			MainPictureBox.Location = new Point(0, 0);
			MainPictureBox.Name = "MainPictureBox";
			MainPictureBox.Size = new Size(479, 479);
			MainPictureBox.TabIndex = 0;
			MainPictureBox.TabStop = false;
			MainPictureBox.DragDrop += Form1_DragDrop;
			MainPictureBox.DragEnter += Form1_DragEnter;
			MainPictureBox.MouseClick += MainPictureBox_MouseClick;
			MainPictureBox.MouseEnter += MainPictureBox_MouseEnter;
			// 
			// ReadButton
			// 
			ReadButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			ReadButton.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			ReadButton.ForeColor = Color.FromArgb(0, 192, 0);
			ReadButton.Location = new Point(398, 493);
			ReadButton.Name = "ReadButton";
			ReadButton.Size = new Size(69, 35);
			ReadButton.TabIndex = 1;
			ReadButton.Text = "Read";
			ReadButton.UseVisualStyleBackColor = true;
			// 
			// ReadResult
			// 
			ReadResult.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			ReadResult.AutoSize = true;
			ReadResult.Font = new Font("メイリオ", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
			ReadResult.Location = new Point(12, 493);
			ReadResult.Name = "ReadResult";
			ReadResult.Size = new Size(44, 20);
			ReadResult.TabIndex = 2;
			ReadResult.Text = "result";
			// 
			// Form1
			// 
			AllowDrop = true;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(479, 540);
			Controls.Add(ReadResult);
			Controls.Add(ReadButton);
			Controls.Add(MainPictureBox);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "Form1";
			Text = "Form1";
			DragDrop += Form1_DragDrop;
			DragEnter += Form1_DragEnter;
			((System.ComponentModel.ISupportInitialize)MainPictureBox).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private PictureBox MainPictureBox;
		private Button ReadButton;
		private Label ReadResult;
	}
}
