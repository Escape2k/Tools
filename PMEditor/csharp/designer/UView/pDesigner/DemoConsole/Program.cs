﻿using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using pF.DesignSurfaceExt;

using Designer;

namespace UView
{
    class Program
    {
        const string _Name_ = "DemoConsole";

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Begin the demo pF p(ico)F(orm)Designer...");

                //- create the Form
                MainForm f = new MainForm();
                var designer = f.Designer as UDesigner;
                f.Size = new Size(1200, 800);

                //- add some tabpages
                Console.WriteLine("I'm trying to add some Surfaces...");
                designer.AddDesignSurface<UserControl>(400, 320, AlignmentModeEnum.SnapLines, new Size(1, 1));
                designer.AddDesignSurface<UserControl>(640, 480, AlignmentModeEnum.SnapLines, new Size(1, 1));
                designer.AddDesignSurface<UserControl>(800, 600, AlignmentModeEnum.SnapLines, new Size(1, 1));
                designer.AddDesignSurface<Form>(1024, 768, AlignmentModeEnum.Grid, new Size(8, 8));
                Console.WriteLine("Surfaces added!");

                //- Add controls to the Forms using IToolboxUser 
                Console.WriteLine("for each Surface I'm trying to add some Controls...");
                int n = 0;
                foreach (IDesignSurfaceExt surf in designer.DesignSurfaceManager.DesignSurfaces)
                {
                    //- use the IToolboxUser interface to add a Button to the root component 
                    CreateControlsUsingTheToolbox(surf);
                    //- create others control via DesignSurfaces
                    CreateControlsUsingTheDesignSurface(surf, ++n);
                }
                Console.WriteLine("Controls added!");

                //- the Tabcontrl.SelectedIndex doesn't raise the event OnSelectedIndexChanged
                designer.TabControlHostingDesignSurfaces.SelectedIndexChanged += (s, e) =>
                {
                    Console.WriteLine("First TabPage selected!");
                };
                designer.TabControlHostingDesignSurfaces.SelectedIndex = 0;
                //- therefore raise it by hand with Reflection
                RaiseEventViaReflection(designer.TabControlHostingDesignSurfaces);

                f.ShowDialog();
                Console.WriteLine("Bye!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception (strike a key to quit!): " + ex.Message);
                if (null != ex.InnerException)
                    Console.WriteLine("Inner Exception : " + ex.InnerException.Message);
                Console.ReadLine();
            }
        }

        private static void RaiseEventViaReflection(TabControl tbc)
        {
            // using System.Reflection;
            Type t = typeof(TabControl);
            object[] p = new object[1];
            p[0] = EventArgs.Empty;
            MethodInfo m = t.GetMethod("OnSelectedIndexChanged", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(tbc, p);
        }

        private static void CreateControlsUsingTheToolbox(IDesignSurfaceExt surface)
        {
            try
            {
                IDesignerHost idh = (IDesignerHost)surface.GetIDesignerHost();
                IToolboxUser itu = (IToolboxUser)idh.GetDesigner(idh.RootComponent);
                itu.ToolPicked(new ToolboxItem(typeof(Button)));

                //- the control just deployed on the Designsurface 
                //- therefore it is still selected 
                //- get the SelectionService
                ISelectionService selectionService = (ISelectionService)(idh.GetService(typeof(ISelectionService)));
                object objCtrlJustPlaced = selectionService.PrimarySelection;
                //- we know its a control
                Control ctrlJustPlaced = (Control)objCtrlJustPlaced;
                ctrlJustPlaced.Size = new Size(((Form)idh.RootComponent).ClientSize.Width - 10, 40);
                ctrlJustPlaced.Location = new Point(0, ((Form)idh.RootComponent).ClientSize.Height - ctrlJustPlaced.Size.Height);
                ctrlJustPlaced.Text = "I'm the Button placed via Toolbox";
                ctrlJustPlaced.BackColor = Color.WhiteSmoke;
            }
            catch (Exception exx)
            {
                Console.WriteLine(" the DesignSurface has generated an Exception: " + exx.Message);
            }
        }

        private static void CreateControlsUsingTheDesignSurface(IDesignSurfaceExt surface, int n)
        {
            try
            {
                Form rootComponent = surface.GetIDesignerHost().RootComponent as Form;
                switch (n)
                {
                    case 1:
                        {
                            rootComponent.BackColor = Color.Gray;
                            rootComponent.Text = "Root Component hosted by the DesignSurface N.1";
                            //- step.3
                            //- create some Controls at DesignTime
                            TextBox t1 = (TextBox)surface.CreateControl(typeof(TextBox), new Size(200, 20), new Point(10, 200));
                            t1.Text = "I'm a TextBox";
                            t1.BackColor = SystemColors.Info;
                            Button b1 = (Button)surface.CreateControl(typeof(Button), new Size(200, 40), new Point(10, 10));
                            Button b2 = (Button)surface.CreateControl(typeof(Button), new Size(200, 40), new Point(100, 100));
                            b1.Text = "I'm the first Button";
                            b2.Text = "I'm the second Button";
                            b1.BackColor = Color.LightGray;
                            b2.BackColor = Color.LightGreen;
                        }
                        break;
                    case 2:
                        {
                            rootComponent.BackColor = Color.Yellow;
                            rootComponent.Text = "Root Component hosted by the DesignSurface N.2";
                            //- step.3
                            //- create some Controls at DesignTime
                            TextBox t1 = (TextBox)surface.CreateControl(typeof(TextBox), new Size(200, 20), new Point(10, 10));
                            Button b1 = (Button)surface.CreateControl(typeof(Button), new Size(200, 40), new Point(10, 40));
                            Label l1 = (Label)surface.CreateControl(typeof(Label), new Size(200, 120), new Point(100, 100));
                            t1.Text = "I'm a TextBox";
                            b1.Text = "I'm a Button";
                            b1.BackColor = Color.Coral;
                            l1.Text = "I'm a Label";
                            l1.BackColor = Color.Coral;
                        }
                        break;
                    case 3:
                        {
                            rootComponent.BackColor = Color.YellowGreen;
                            rootComponent.Text = "Root Component hosted by the DesignSurface N.3";
                            //- step.3
                            //- create some Controls at DesignTime
                            Button b1 = (Button)surface.CreateControl(typeof(Button), new Size(200, 40), new Point(10, 10));
                            Button b2 = (Button)surface.CreateControl(typeof(Button), new Size(200, 40), new Point(100, 100));
                            Button b3 = (Button)surface.CreateControl(typeof(Button), new Size(200, 40), new Point(22, 22));
                            b1.Text = "I'm the first Button";
                            b2.Text = "I'm the second Button";
                            b3.Text = "I'm the third Button (belonging to the GroupBox)";
                            GroupBox gb = (GroupBox)surface.CreateControl(typeof(GroupBox), new Size(300, 180), new Point(100, 200));
                            b3.Parent = gb;
                            b3.BackColor = Color.LightGray;
                        }
                        break;
                    case 4:
                        {
                            rootComponent.BackColor = Color.Orange;
                            rootComponent.Text = "Root Component hosted by the DesignSurface N.4";       //- step.1
                            //- step.3
                            //- create some Controls at DesignTime
                            Button b1 = (Button)surface.CreateControl(typeof(Button), new Size(200, 40), new Point(10, 10));
                            Button b2 = (Button)surface.CreateControl(typeof(Button), new Size(200, 40), new Point(100, 100));
                            b1.Text = "I'm the first Button";
                            b2.Text = "I'm the second Button";
                            b1.BackColor = Color.Gold;
                            b2.BackColor = Color.LightGreen;
                        }
                        break;
                    default: Console.WriteLine("Invalid selection"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" the DesignSurface N. " + n.ToString() + " has generated an Exception: " + ex.Message);
            }
        }
    }
}

