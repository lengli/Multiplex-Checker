
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	
	private global::Gtk.Action FileAction;
	
	private global::Gtk.Action AboutAction;
	
	private global::Gtk.Action openAction;
	
	private global::Gtk.Action openAct;
	
	private global::Gtk.Fixed fixed1;
	
	private global::Gtk.MenuBar menubar1;
	
	private global::Gtk.Fixed fixed2;
	
	private global::Gtk.Button runBtn;
	
	private global::Gtk.Label nSpecLbl;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.Label label4;
	
	private global::Gtk.Label FileNameLbl;
	
	private global::Gtk.Label label6;
	
	private global::Gtk.Label label7;
	
	private global::Gtk.Label label11;
	
	private global::Gtk.Label label12;
	
	private global::Gtk.Label label10;
	
	private global::Gtk.Fixed fixed3;
	
	private global::Gtk.Label indexLbl;
	
	private global::Gtk.ComboBox mtpxCombo;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.ScrolledWindow scrolledwindow1;
	
	private global::Gtk.Label infoLbl;
	
	private global::Gtk.Label label5;
	
	private global::Gtk.Label retLbl;
	
	private global::Gtk.HSeparator hseparator1;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.FileAction = new global::Gtk.Action ("FileAction", global::Mono.Unix.Catalog.GetString ("File"), null, null);
		this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("File");
		w1.Add (this.FileAction, null);
		this.AboutAction = new global::Gtk.Action ("AboutAction", global::Mono.Unix.Catalog.GetString ("About"), null, null);
		this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("About");
		w1.Add (this.AboutAction, null);
		this.openAction = new global::Gtk.Action ("openAction", global::Mono.Unix.Catalog.GetString ("Open MS1 File"), null, "gtk-open");
		this.openAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Open MS1 File");
		w1.Add (this.openAction, null);
		this.openAct = new global::Gtk.Action ("openAct", global::Mono.Unix.Catalog.GetString ("Open File"), null, "gtk-open");
		this.openAct.ShortLabel = global::Mono.Unix.Catalog.GetString ("Open File");
		w1.Add (this.openAct, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.BorderWidth = ((uint)(3));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.fixed1 = new global::Gtk.Fixed ();
		this.fixed1.Name = "fixed1";
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.fixed2 = new global::Gtk.Fixed();
		this.fixed2.WidthRequest = 600;
		this.fixed2.HeightRequest = 210;
		this.fixed2.Name = "fixed2";
		this.fixed2.HasWindow = false;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.runBtn = new global::Gtk.Button ();
		this.runBtn.CanFocus = true;
		this.runBtn.Name = "runBtn";
		this.runBtn.UseUnderline = true;
		this.runBtn.Label = global::Mono.Unix.Catalog.GetString ("Run");
		this.fixed2.Add (this.runBtn);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed2 [this.runBtn]));
		w2.Y = 150;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.nSpecLbl = new global::Gtk.Label();
		this.nSpecLbl.Name = "nSpecLbl";
		this.nSpecLbl.Xalign = 0F;
		this.nSpecLbl.LabelProp = global::Mono.Unix.Catalog.GetString("nSpec");
		this.fixed2.Add(this.nSpecLbl);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.nSpecLbl]));
		w3.X = 110;
		w3.Y = 30;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label1 = new global::Gtk.Label();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Threshold:");
		this.fixed2.Add(this.label1);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.label1]));
		w4.Y = 60;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label2 = new global::Gtk.Label();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Filename:");
		this.fixed2.Add(this.label2);
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label4 = new global::Gtk.Label();
		this.label4.Name = "label4";
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("N. of Spectrum:");
		this.fixed2.Add(this.label4);
		global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.label4]));
		w6.Y = 30;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.FileNameLbl = new global::Gtk.Label();
		this.FileNameLbl.Name = "FileNameLbl";
		this.FileNameLbl.LabelProp = global::Mono.Unix.Catalog.GetString("label4");
		this.fixed2.Add(this.FileNameLbl);
		global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.FileNameLbl]));
		w7.X = 110;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label6 = new global::Gtk.Label();
		this.label6.Name = "label6";
		this.label6.LabelProp = global::Mono.Unix.Catalog.GetString("N. of Top Peaks:");
		this.fixed2.Add(this.label6);
		global::Gtk.Fixed.FixedChild w8 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.label6]));
		w8.Y = 90;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label7 = new global::Gtk.Label();
		this.label7.Name = "label7";
		this.label7.LabelProp = global::Mono.Unix.Catalog.GetString("Window");
		this.fixed2.Add(this.label7);
		global::Gtk.Fixed.FixedChild w9 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.label7]));
		w9.X = 300;
		w9.Y = 30;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label11 = new global::Gtk.Label();
		this.label11.Name = "label11";
		this.label11.LabelProp = global::Mono.Unix.Catalog.GetString("Right Size:");
		this.fixed2.Add(this.label11);
		global::Gtk.Fixed.FixedChild w10 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.label11]));
		w10.X = 360;
		w10.Y = 60;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label12 = new global::Gtk.Label();
		this.label12.Name = "label12";
		this.label12.LabelProp = global::Mono.Unix.Catalog.GetString("Use prediction");
		this.fixed2.Add(this.label12);
		global::Gtk.Fixed.FixedChild w11 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.label12]));
		w11.Y = 120;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label10 = new global::Gtk.Label();
		this.label10.Name = "label10";
		this.label10.LabelProp = global::Mono.Unix.Catalog.GetString("Left Size:");
		this.fixed2.Add(this.label10);
		global::Gtk.Fixed.FixedChild w12 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.label10]));
		w12.X = 360;
		w12.Y = 30;
		this.fixed1.Add(this.fixed2);
		global::Gtk.Fixed.FixedChild w13 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.fixed2]));
		w13.X = 20;
		w13.Y = 35;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.fixed3 = new global::Gtk.Fixed();
		this.fixed3.WidthRequest = 500;
		this.fixed3.HeightRequest = 300;
		this.fixed3.Name = "fixed3";
		this.fixed3.HasWindow = false;
		// Container child fixed3.Gtk.Fixed+FixedChild
		this.indexLbl = new global::Gtk.Label();
		this.indexLbl.Name = "indexLbl";
		this.indexLbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Scan Index:");
		this.fixed3.Add(this.indexLbl);
		global::Gtk.Fixed.FixedChild w14 = ((global::Gtk.Fixed.FixedChild)(this.fixed3[this.indexLbl]));
		w14.Y = 8;
		// Container child fixed3.Gtk.Fixed+FixedChild
		this.mtpxCombo = global::Gtk.ComboBox.NewText();
		this.mtpxCombo.Name = "mtpxCombo";
		this.fixed3.Add(this.mtpxCombo);
		global::Gtk.Fixed.FixedChild w15 = ((global::Gtk.Fixed.FixedChild)(this.fixed3[this.mtpxCombo]));
		w15.X = 72;
		// Container child fixed3.Gtk.Fixed+FixedChild
		this.label3 = new global::Gtk.Label();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Scan information");
		this.fixed3.Add(this.label3);
		global::Gtk.Fixed.FixedChild w16 = ((global::Gtk.Fixed.FixedChild)(this.fixed3[this.label3]));
		w16.Y = 85;
		// Container child fixed3.Gtk.Fixed+FixedChild
		this.scrolledwindow1 = new global::Gtk.ScrolledWindow();
		this.scrolledwindow1.WidthRequest = 450;
		this.scrolledwindow1.HeightRequest = 150;
		this.scrolledwindow1.CanFocus = true;
		this.scrolledwindow1.Name = "scrolledwindow1";
		this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child scrolledwindow1.Gtk.Container+ContainerChild
		global::Gtk.Viewport w17 = new global::Gtk.Viewport();
		w17.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child GtkViewport.Gtk.Container+ContainerChild
		this.infoLbl = new global::Gtk.Label();
		this.infoLbl.Name = "infoLbl";
		this.infoLbl.Xalign = 0.04F;
		this.infoLbl.Yalign = 0.04F;
		this.infoLbl.LabelProp = global::Mono.Unix.Catalog.GetString("-");
		w17.Add(this.infoLbl);
		this.scrolledwindow1.Add(w17);
		this.fixed3.Add(this.scrolledwindow1);
		global::Gtk.Fixed.FixedChild w20 = ((global::Gtk.Fixed.FixedChild)(this.fixed3[this.scrolledwindow1]));
		w20.Y = 107;
		// Container child fixed3.Gtk.Fixed+FixedChild
		this.label5 = new global::Gtk.Label();
		this.label5.Name = "label5";
		this.label5.LabelProp = global::Mono.Unix.Catalog.GetString("Retention Time: ");
		this.fixed3.Add(this.label5);
		global::Gtk.Fixed.FixedChild w21 = ((global::Gtk.Fixed.FixedChild)(this.fixed3[this.label5]));
		w21.Y = 50;
		// Container child fixed3.Gtk.Fixed+FixedChild
		this.retLbl = new global::Gtk.Label();
		this.retLbl.Name = "retLbl";
		this.retLbl.LabelProp = global::Mono.Unix.Catalog.GetString("-");
		this.fixed3.Add(this.retLbl);
		global::Gtk.Fixed.FixedChild w22 = ((global::Gtk.Fixed.FixedChild)(this.fixed3[this.retLbl]));
		w22.X = 99;
		w22.Y = 50;
		this.fixed1.Add(this.fixed3);
		global::Gtk.Fixed.FixedChild w23 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.fixed3]));
		w23.X = 20;
		w23.Y = 265;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='openAct' action='openAct'/></menu><menu name='AboutAction' action='AboutAction'/></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
		this.menubar1.WidthRequest = 683;
		this.menubar1.Name = "menubar1";
		this.fixed1.Add (this.menubar1);
		this.hseparator1 = new global::Gtk.HSeparator();
		this.hseparator1.WidthRequest = 660;
		this.hseparator1.Name = "hseparator1";
		this.fixed1.Add(this.hseparator1);
		global::Gtk.Fixed.FixedChild w24 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.hseparator1]));
		w24.X = 20;
		w24.Y = 240;
		this.Add (this.fixed1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 706;
		this.DefaultHeight = 700;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
	}
}
