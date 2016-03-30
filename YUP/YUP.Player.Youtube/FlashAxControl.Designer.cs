namespace YUP.Player.Youtube
{
    partial class FlashAxControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlashAxControl));
            this.YTplayer = new AxShockwaveFlashObjects.AxShockwaveFlash();
            ((System.ComponentModel.ISupportInitialize)(this.YTplayer)).BeginInit();
            this.SuspendLayout();
            // 
            // YTplayer
            // 
            this.YTplayer.Enabled = true;
            this.YTplayer.Location = new System.Drawing.Point(0, 0);
            this.YTplayer.Name = "YTplayer";
            this.YTplayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("YTplayer.OcxState")));
            this.YTplayer.Size = new System.Drawing.Size(644, 453);
            this.YTplayer.TabIndex = 0;
            this.YTplayer.FSCommand += new AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEventHandler(this.YTplayer_FSCommand);
            this.YTplayer.FlashCall += new AxShockwaveFlashObjects._IShockwaveFlashEvents_FlashCallEventHandler(this.YTplayer_FlashCall);
            // 
            // FlashAxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.YTplayer);
            this.Name = "FlashAxControl";
            this.Size = new System.Drawing.Size(644, 453);
            ((System.ComponentModel.ISupportInitialize)(this.YTplayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxShockwaveFlashObjects.AxShockwaveFlash YTplayer;
    }
}
