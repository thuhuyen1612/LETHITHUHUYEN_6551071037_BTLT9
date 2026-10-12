using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using FastReport;
using FastReport.Export.Image;
using FastReport.Export.PdfSimple;

namespace Cau1_TiemTapHoa
{
    public class FormReportPreview : Form
    {
        private readonly Report _report;
        private readonly List<Image> _pageImages = new List<Image>();
        private int _currentPageIndex = 0;
        private float _zoomFactor = 1.0f;

        private ToolStrip _toolStrip = null!;
        private ToolStripButton _btnPrint = null!;
        private ToolStripButton _btnExportPdf = null!;
        private ToolStripSeparator _sep1 = null!;
        private ToolStripButton _btnZoomOut = null!;
        private ToolStripComboBox _cmbZoom = null!;
        private ToolStripButton _btnZoomIn = null!;
        private ToolStripSeparator _sep2 = null!;
        private ToolStripButton _btnFirst = null!;
        private ToolStripButton _btnPrev = null!;
        private ToolStripLabel _lblPage = null!;
        private ToolStripButton _btnNext = null!;
        private ToolStripButton _btnLast = null!;

        private Panel _pnlScroll = null!;
        private PictureBox _picPage = null!;

        public FormReportPreview(Report report, string title = "FastReport Preview")
        {
            _report = report;
            Text = title;
            Size = new Size(950, 750);
            StartPosition = FormStartPosition.CenterParent;

            InitializeComponents();
            RenderPages();
            UpdateView();
        }

        private void InitializeComponents()
        {
            _toolStrip = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                Padding = new Padding(5, 2, 5, 2)
            };

            _btnPrint = new ToolStripButton("In (Print)")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                ToolTipText = "In báo cáo"
            };
            _btnPrint.Click += BtnPrint_Click;

            _btnExportPdf = new ToolStripButton("Xuất PDF")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                ToolTipText = "Xuất tài liệu sang PDF"
            };
            _btnExportPdf.Click += BtnExportPdf_Click;

            _sep1 = new ToolStripSeparator();

            _btnZoomOut = new ToolStripButton(" - ") { ToolTipText = "Thu nhỏ" };
            _btnZoomOut.Click += (s, e) => { SetZoom(_zoomFactor - 0.15f); };

            _cmbZoom = new ToolStripComboBox { Width = 80, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbZoom.Items.AddRange(new object[] { "50%", "75%", "100%", "125%", "150%", "200%" });
            _cmbZoom.SelectedItem = "100%";
            _cmbZoom.SelectedIndexChanged += (s, e) =>
            {
                if (float.TryParse(_cmbZoom.SelectedItem?.ToString()?.Replace("%", ""), out float pct))
                {
                    _zoomFactor = pct / 100.0f;
                    UpdateImageScale();
                }
            };

            _btnZoomIn = new ToolStripButton(" + ") { ToolTipText = "Phóng to" };
            _btnZoomIn.Click += (s, e) => { SetZoom(_zoomFactor + 0.15f); };

            _sep2 = new ToolStripSeparator();

            _btnFirst = new ToolStripButton("|<") { ToolTipText = "Trang đầu" };
            _btnFirst.Click += (s, e) => { GoToPage(0); };

            _btnPrev = new ToolStripButton("<") { ToolTipText = "Trang trước" };
            _btnPrev.Click += (s, e) => { GoToPage(_currentPageIndex - 1); };

            _lblPage = new ToolStripLabel("1 / 1");

            _btnNext = new ToolStripButton(">") { ToolTipText = "Trang sau" };
            _btnNext.Click += (s, e) => { GoToPage(_currentPageIndex + 1); };

            _btnLast = new ToolStripButton(">|") { ToolTipText = "Trang cuối" };
            _btnLast.Click += (s, e) => { GoToPage(_pageImages.Count - 1); };

            _toolStrip.Items.AddRange(new ToolStripItem[]
            {
                _btnPrint, _btnExportPdf, _sep1,
                _btnZoomOut, _cmbZoom, _btnZoomIn, _sep2,
                _btnFirst, _btnPrev, _lblPage, _btnNext, _btnLast
            });

            _pnlScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(140, 144, 148)
            };

            _picPage = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Location = new Point(20, 20)
            };
            _picPage.Paint += (s, e) =>
            {
                // Draw paper border & shadow
                ControlPaint.DrawBorder(e.Graphics, _picPage.ClientRectangle, Color.DarkGray, ButtonBorderStyle.Solid);
            };

            _pnlScroll.Controls.Add(_picPage);
            Controls.Add(_pnlScroll);
            Controls.Add(_toolStrip);
        }

        private void RenderPages()
        {
            _pageImages.Clear();
            string tempDir = Path.Combine(Path.GetTempPath(), "FR_Preview_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                ImageExport imgExport = new ImageExport
                {
                    ImageFormat = ImageExportFormat.Png,
                    Resolution = 150,
                    SeparateFiles = true
                };

                string outPattern = Path.Combine(tempDir, "page.png");
                _report.Export(imgExport, outPattern);

                var files = Directory.GetFiles(tempDir, "*.png");
                Array.Sort(files);
                foreach (var file in files)
                {
                    byte[] bytes = File.ReadAllBytes(file);
                    using var ms = new MemoryStream(bytes);
                    _pageImages.Add(Image.FromStream(ms));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try { Directory.Delete(tempDir, true); } catch { }
            }

            if (_pageImages.Count == 0)
            {
                Bitmap blank = new Bitmap(800, 1100);
                using (Graphics g = Graphics.FromImage(blank))
                {
                    g.Clear(Color.White);
                    g.DrawString("Không có trang nào để hiển thị", new Font("Arial", 14), Brushes.Black, 20, 20);
                }
                _pageImages.Add(blank);
            }
        }

        private void SetZoom(float zoom)
        {
            if (zoom < 0.3f) zoom = 0.3f;
            if (zoom > 3.0f) zoom = 3.0f;
            _zoomFactor = zoom;
            _cmbZoom.Text = $"{(int)(_zoomFactor * 100)}%";
            UpdateImageScale();
        }

        private void GoToPage(int index)
        {
            if (index >= 0 && index < _pageImages.Count)
            {
                _currentPageIndex = index;
                UpdateView();
            }
        }

        private void UpdateView()
        {
            _lblPage.Text = $"{_currentPageIndex + 1} / {_pageImages.Count}";
            _btnFirst.Enabled = _btnPrev.Enabled = _currentPageIndex > 0;
            _btnLast.Enabled = _btnNext.Enabled = _currentPageIndex < _pageImages.Count - 1;
            UpdateImageScale();
        }

        private void UpdateImageScale()
        {
            if (_currentPageIndex < 0 || _currentPageIndex >= _pageImages.Count) return;

            Image img = _pageImages[_currentPageIndex];
            int w = (int)(img.Width * _zoomFactor * (96f / 150f));
            int h = (int)(img.Height * _zoomFactor * (96f / 150f));

            _picPage.Size = new Size(w, h);
            _picPage.Image = img;

            // Center in scroll panel
            int posX = Math.Max(20, (_pnlScroll.ClientSize.Width - w) / 2);
            _picPage.Location = new Point(posX, 20);
        }

        private void BtnExportPdf_Click(object? sender, EventArgs e)
        {
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "Xuất PDF",
                FileName = "BangGia.pdf"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    _report.Export(pdfExport, sfd.FileName);

                    var res = MessageBox.Show("Xuất file PDF thành công! Bạn có muốn mở file ngay không?",
                        "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            try
            {
                using PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    using PrintDocument doc = new PrintDocument();
                    int printPage = 0;
                    doc.PrinterSettings = pd.PrinterSettings;
                    doc.PrintPage += (s, ev) =>
                    {
                        if (printPage < _pageImages.Count)
                        {
                            Image img = _pageImages[printPage];
                            ev.Graphics?.DrawImage(img, ev.MarginBounds);
                            printPage++;
                            ev.HasMorePages = printPage < _pageImages.Count;
                        }
                    };
                    doc.Print();
                    MessageBox.Show("Đã gửi lệnh in thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in: " + ex.Message, "Lỗi in", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var img in _pageImages)
                {
                    img.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
