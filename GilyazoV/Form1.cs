using System.Windows.Forms;

namespace GilyazoV
{
    public partial class Form1 : Form
    {
        public CHashTableList htl = new CHashTableList(3);
        public Semantic sem;

        public Form1()
        {
            InitializeComponent();
            //tbFSource.AppendText("((acbd(100010,000100,000100010)))" + "\r\n");
            tbFSource.AppendText("int ad := 000101110;ad<ad" + "\r\n");
            int n = tbFSource.Lines.Length;



        }

        public void TablesToMemo(object sender, System.EventArgs e)
        {
            List<string> listTable = new List<string>();

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            htl.TableToStringList(0, listTable);
            for (int i = 0; i < listTable.Count; i++)
                listBox1.Items.Add(listTable[i]);
            listTable.Clear();

            htl.TableToStringList(1, listTable);
            for (int i = 0; i < listTable.Count; i++)
                listBox2.Items.Add(listTable[i]);
            listTable.Clear();

            htl.TableToStringList(2, listTable);
            for (int i = 0; i < listTable.Count; i++)
                listBox3.Items.Add(listTable[i]);
            listTable.Clear();
        }


        private void btnFStart_Click(object sender, EventArgs e)
        {
            tbFMessage.Clear();

            uSyntAnalyzer Synt = new uSyntAnalyzer();
            Synt.tree = syntTree;
            Synt.Lex.strPSource = tbFSource.Lines;
            Synt.Lex.strPMessage = tbFMessage.Lines;
            Synt.Lex.intPSourceColSelection = -1;
            Synt.Lex.intPSourceRowSelection = 0;
            Synt.Lex.enumPState = TState.Start;
            try
            {
                Synt.Lex.NextToken();
                Synt.S();
            }
            catch (Exception exc)
            {
                if (exc.Message == "" || exc.Message == null)
                {
                    tbFMessage.Text += "Неизвестная ошибка";
                }
                else
                {
                    tbFMessage.Text += exc.Message;
                }
                tbFSource.Select();
                tbFSource.SelectionStart = 0;
                int n = 0;
                for (int i = 0; i < Synt.Lex.intPSourceRowSelection; i++)
                {
                    n += tbFSource.Lines[i].Length + 2;
                }
                n += Synt.Lex.intPSourceColSelection;
                tbFSource.SelectionLength = n;
            }

        }

        private void btnFRecord_Click(object sender, EventArgs e)
        {
            CLex Lex = new CLex();
            Lex.strPSource = tbFSource.Lines;
            Lex.strPMessage = tbFMessage.Lines;
            Lex.intPSourceColSelection = 0;
            Lex.intPSourceRowSelection = 0;
            Generator gen = new Generator();
            gen.Restruct(syntTree, structTree);
            int x = tbFSource.TextLength;
            int y = tbFSource.Lines.Length;
            tbFMessage.Text = "";
            try
            {
                while (Lex.enumPState != TState.Finish)
                {
                    Lex.NextToken();
                    string s1 = "", s = "";
                    switch (Lex.enumPToken)
                    {
                        case TToken.lxmIdentifier:
                            {
                                s1 = "id " + Lex.strPLexicalUnit; int b = 0;
                                if (htl.AddLexicalUnit(Lex.strPLexicalUnit, 0, ref b))
                                {
                                    TablesToMemo(this, e);
                                }
                                break;
                            }
                        case TToken.lxmNumber:
                            {
                                s1 = "num " + Lex.strPLexicalUnit; int b = 0;
                                if (htl.AddLexicalUnit(Lex.strPLexicalUnit, 1, ref b))
                                {
                                    TablesToMemo(this, e);
                                }
                                break;
                            }
                        case TToken.lxmRightParenth:
                        case TToken.lxmLeftParenth:
                        case TToken.lxmComma:
                        case TToken.lxmInt:
                        case TToken.lxmOr:
                        case TToken.lxmAnd:
                        case TToken.lxmAssign:
                        case TToken.lxmLess:
                        case TToken.lxmGreater:
                        case TToken.lxmtz:
                        case TToken.lxmDD:
                        case TToken.lxmDot:
                        case TToken.lxmDollar:
                        case TToken.lxmMinus:
                        case TToken.lxmPlus:
                        case TToken.lxmExclamation:
                        case TToken.lxmQuestion:
                        case TToken.lxmLeftParenthSqr:
                        case TToken.lxmRightParenthSqr:
                            {
                                Reserved(ref s1, Lex, e);
                                break;
                            }
                    }
                    String m = "(" + s + "" + s1 + ")";
                    tbFMessage.Text += m;
                }
            }
            catch (Exception exc)
            {
                tbFMessage.Text += exc.Message;
                tbFSource.Select();
                tbFSource.SelectionStart = 0;
                int n = 0;
                for (int i = 0; i < Lex.intPSourceRowSelection; i++) n += tbFSource.Lines[i].Length + 2;
                n += Lex.intPSourceColSelection;
                tbFSource.SelectionLength = n;
            }
        }

        void Reserved(ref string s1, CLex Lex, EventArgs e)
        {
            s1 = "res '" + Lex.strPLexicalUnit + "'";
            int b = 0;
            if (htl.AddLexicalUnit(Lex.strPLexicalUnit, 2, ref b))
            {
                TablesToMemo(this, e);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                if (htl.DeleteLexicalUnit(listBox1.Items[listBox1.SelectedIndex].ToString(), 0))
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    listBox1.SelectedIndex = -1;
                }
            }
            if (listBox2.SelectedIndex >= 0)
            {
                if (htl.DeleteLexicalUnit(listBox2.Items[listBox2.SelectedIndex].ToString(), 1))
                {
                    listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                    listBox2.SelectedIndex = -1;
                }
            }
            if (listBox3.SelectedIndex >= 0)
            {
                if (htl.DeleteLexicalUnit(listBox3.Items[listBox3.SelectedIndex].ToString(), 2))
                {
                    listBox3.Items.RemoveAt(listBox3.SelectedIndex);
                    listBox3.SelectedIndex = -1;
                }
            }

        }

        private void findBtn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int d = 0;
                if (htl.SearchLexicalUnit(listBox1.Items[listBox1.SelectedIndex].ToString(), 0, ref d))
                {
                    tbFMessage.Text = d.ToString();
                }
                else
                {
                    tbFMessage.Text = "Не удалось найти индекс элемента";
                }
            }
            if (listBox2.SelectedIndex >= 0)
            {
                int d = 0;
                if (htl.SearchLexicalUnit(listBox2.Items[listBox2.SelectedIndex].ToString(), 1, ref d))
                {
                    tbFMessage.Text = d.ToString();
                }
                else
                {
                    tbFMessage.Text = "Не удалось найти индекс элемента";
                }
            }
            if (listBox3.SelectedIndex >= 0)
            {
                int d = 0;
                if (htl.SearchLexicalUnit(listBox3.Items[listBox3.SelectedIndex].ToString(), 2, ref d))
                {
                    tbFMessage.Text = d.ToString();
                }
                else
                {
                    tbFMessage.Text = "Не удалось найти индекс элемента";
                }
            }

        }

        private void ChngBtn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                htl.ChangeLexicalUnit(listBox1.Items[listBox1.SelectedIndex].ToString(), 0, tbFMessage.Text);
                List<string> listTable = new List<string>();

                listBox1.Items.Clear();

                htl.TableToStringList(0, listTable);
                for (int i = 0; i < listTable.Count; i++)
                    listBox1.Items.Add(listTable[i]);
                listTable.Clear();
            }
            if (listBox2.SelectedIndex >= 0)
            {
                htl.ChangeLexicalUnit(listBox2.Items[listBox2.SelectedIndex].ToString(), 1, tbFMessage.Text);
                List<string> listTable = new List<string>();

                listBox2.Items.Clear();

                htl.TableToStringList(1, listTable);
                for (int i = 0; i < listTable.Count; i++)
                    listBox2.Items.Add(listTable[i]);
                listTable.Clear();
            }
            if (listBox3.SelectedIndex >= 0)
            {
                htl.ChangeLexicalUnit(listBox3.Items[listBox3.SelectedIndex].ToString(), 2, tbFMessage.Text);
                List<string> listTable = new List<string>();

                listBox3.Items.Clear();

                htl.TableToStringList(2, listTable);
                for (int i = 0; i < listTable.Count; i++)
                    listBox3.Items.Add(listTable[i]);
                listTable.Clear();
            }

        }
    }
}
