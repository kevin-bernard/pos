using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace accouting_system_manager.Components
{
    public partial class MenuItem : UserControl
    {
        [Category("Action"), Browsable(true)]
        public event OnItemClicked onItemClicked;

        public delegate void OnItemClicked(object sender, EventArgs e);

        private Color itemColor;
        private Color itemActiveColor;
        private Color selectedBackgroundColor;
        private string itemText;
        private Image image;
        private Image activeImage;
        
        [Category("Appearance"), Browsable(true)]
        public Color ItemColor {
            get {
                return itemColor;
            } set
            {
                itemColor = value;
                linkLabel1.LinkColor = itemColor;
            }
        }

        [Category("Appearance"), Browsable(true)]
        public Color ItemActiveColor
        {
            get
            {
                return itemActiveColor;
            }
            set
            {
                itemActiveColor = value;
                linkLabel1.ActiveLinkColor = itemActiveColor;
            }
        }

        [Category("Appearance"), Browsable(true)]
        public Color SelectedBackgroundColor
        {
            get
            {
                return selectedBackgroundColor;
            }
            set
            {
                selectedBackgroundColor = value;
            }
        }


        [Category("Appearance"), Browsable(true)]
        public Image Image
        {
            get {
                return image;
            }

            set {
                image = value;
                pctBox.BackgroundImage = image;
            }
        }

        [Category("Appearance"), Browsable(true)]
        public Image ActiveImage
        {
            get
            {
                return activeImage;
            }

            set
            {
                activeImage = value;
            }
        }

        [Category("Appearance"), Browsable(true)]
        public string ItemText
        {
            get {
                return itemText;
            }

            set
            {
                itemText = value;
                linkLabel1.Text = itemText;
            }
        }

        public bool IsSelected {
            get
            {
                return BackColor == selectedBackgroundColor;
            }
        }

        public void SelectItem()
        {
            BackColor = selectedBackgroundColor;
        }

        public void UnSelectItem()
        {
            BackColor = Color.Transparent;
        }

        public MenuItem()
        {
            InitializeComponent();
        }

        private void on_MouseHover(object sender, EventArgs e)
        {
            pctBox.BackgroundImage = activeImage;
            linkLabel1.LinkColor = itemActiveColor;
        }

        private void on_MouseLeave(object sender, EventArgs e)
        {
            pctBox.BackgroundImage = image;
            linkLabel1.LinkColor = itemColor;
        }

        private void on_Click(object sender, EventArgs e) {
            onItemClicked?.Invoke(sender, e);
            SelectItem();
        }
    }
}
