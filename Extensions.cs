using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackjackIDE {
    public interface IBlackjackControl {
        string source { get; }
        string id { get; }
        int xPriority { get; }
        int yPriority { get; }
        int xWeight { get; }
        int yWeight { get; }
        void ResetLayout();
    }
    public abstract class BlackjackTextBox : TextBox, IBlackjackControl {
        public string source { get; protected set; }
        public string id { get; protected set; }
        public int xPriority { get; set; }
        public int yPriority { get; set; }
        public int xWeight { get; set; }
        public int yWeight { get; set; }
        public abstract void ResetLayout();
        public BlackjackTextBox(string id, string source = "Blackjack") {
            this.id = id;
            this.source = source;
        }
    }
    public class BlackjackMainTextBox : BlackjackTextBox {
        public override void ResetLayout() {
            xPriority = 0;
            yPriority = 0;
            xWeight = 75;
            yWeight = 100;
        }
        public BlackjackMainTextBox() : base("BlackjackMainTextBox") {}
    }
    public class BlackjackToolStripRenderer : ToolStripRenderer {

    }
}
