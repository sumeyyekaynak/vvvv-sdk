#region licence/info

//////project name
//ScreenInfo

//////description
//Retreive information about the actual display

//////licence
//GNU Lesser General Public License (LGPL)
//english: http://www.gnu.org/licenses/lgpl.html
//german: http://www.gnu.de/lgpl-ger.html

//////language/ide
//C# sharpdevelop 

//////dependencies
//VVVV.PluginInterfaces.V1;

//////initial author
//woei

#endregion licence/info

//use what you need
using System;
using System.Drawing;
using VVVV.PluginInterfaces.V1;

//the vvvv node namespace
namespace VVVV.Nodes
{
	
	//class definition
	public class ScreenInfo: IPlugin, IDisposable
    {	          	
    	#region field declaration
    	
    	//the host (mandatory)
    	private IPluginHost FHost;
    	// Track whether Dispose has been called.
   		private bool FDisposed = false;
    	
    	//input pin declaration
    	private IValueIn FRefresh;
    	
    	//output pin declaration
    	private IValueOut FResolutionOutput;
    	private IValueOut FResolutionOffsetOutput;
    	private IValueOut FWorkAreaOutput;
    	private IValueOut FWorkAreaOffsetOutput;
    	private IValueOut FBitsPerPixel;
    	private IValueOut FIsPrimary;
		
    	#endregion field declaration
       
    	#region constructor/destructor
    	
        public ScreenInfo()
        {
			//the nodes constructor
			//nothing to declare for this node
		}
        
        // Implementing IDisposable's Dispose method.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
        	Dispose(true);
        	// Take yourself off the Finalization queue
        	// to prevent finalization code for this object
        	// from executing a second time.
        	GC.SuppressFinalize(this);
        }
        
        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
        	// Check to see if Dispose has already been called.
        	if(!FDisposed)
        	{
        		if(disposing)
        		{
        			// Dispose managed resources.
        		}
        		// Release unmanaged resources. If disposing is false,
        		// only the following code is executed.
	        	
        		FHost.Log(TLogType.Debug, "ScreenInfo is being deleted");
        		
        		// Note that this is not thread safe.
        		// Another thread could start disposing the object
        		// after the managed resources are disposed,
        		// but before the disposed flag is set to true.
        		// If thread safety is necessary, it must be
        		// implemented by the client.
        	}
        	FDisposed = true;
        }
        
        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~ScreenInfo()
        {
        	// Do not re-create Dispose clean-up code here.
        	// Calling Dispose(false) is optimal in terms of
        	// readability and maintainability.
        	Dispose(false);
        }

        #endregion constructor/destructor
        
        #region node name and infos
       
        //provide node infos 
        public static IPluginInfo PluginInfo
	    {
	        get 
	        {
	        	//fill out nodes info
	        	IPluginInfo Info = new PluginInfo();
	        	Info.Name = "ScreenInfo";
	        	Info.Category = "Windows";
	        	Info.Version = "";
	        	Info.Help = "Retreive information about the actual display";
	        	Info.Bugs = "";
	        	Info.Credits = "";
	        	Info.Warnings = "";
	        	
	        	//leave below as is
	        	System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
   				System.Diagnostics.StackFrame sf = st.GetFrame(0);
   				System.Reflection.MethodBase method = sf.GetMethod();
   				Info.Namespace = method.DeclaringType.Namespace;
   				Info.Class = method.DeclaringType.Name;
   				return Info;
   				//leave above as is
	        }
		}

        public bool AutoEvaluate
        {
        	//return true if this node needs to calculate every frame even if nobody asks for its output
        	get {return false;}
        }
        
        #endregion node name and infos
        
      	#region pin creation
        
        //this method is called by vvvv when the node is created
        public void SetPluginHost(IPluginHost Host)
	    {
        	//assign host
	    	FHost = Host;

	    	//create inputs
	    	FHost.CreateValueInput("Refresh", 1, null, TSliceMode.Single, TPinVisibility.True, out FRefresh);
	    	FRefresh.SetSubType(0,1,1,0,true,false, false);
	    	
	    	//create outputs	    	
	    	FHost.CreateValueOutput("Resolution", 2, null, TSliceMode.Dynamic, TPinVisibility.True, out FResolutionOutput);
	    	FResolutionOutput.SetSubType2D(double.MinValue, double.MaxValue, 0.01, 0, 0, false, false, true);
	    	
	    	FHost.CreateValueOutput("Resolution Offset", 2, null, TSliceMode.Dynamic, TPinVisibility.True, out FResolutionOffsetOutput);
	    	FResolutionOffsetOutput.SetSubType2D(double.MinValue, double.MaxValue, 0.01, 0, 0, false, false, true);
	    	
	    	FHost.CreateValueOutput("Working Area", 2, null, TSliceMode.Dynamic, TPinVisibility.True, out FWorkAreaOutput);
	    	FWorkAreaOutput.SetSubType2D(double.MinValue, double.MaxValue, 0.01, 0, 0, false, false, true);
	    	
	    	FHost.CreateValueOutput("Working Area Offset", 2, null, TSliceMode.Dynamic, TPinVisibility.True, out FWorkAreaOffsetOutput);
	    	FWorkAreaOffsetOutput.SetSubType2D(double.MinValue, double.MaxValue, 0.01, 0, 0, false, false, true);
	    	
	    	FHost.CreateValueOutput("Bits Per Pixel", 1, null, TSliceMode.Dynamic, TPinVisibility.True, out FBitsPerPixel);
	    	FBitsPerPixel.SetSubType(0.0, 1.1, 0.01, 0, false, false, true);
	    	
	    	FHost.CreateValueOutput("Is Primary", 1, null, TSliceMode.Dynamic, TPinVisibility.True, out FIsPrimary);
	    	FIsPrimary.SetSubType(0.0, 1.1, 0.01, 0, false, true, false);
	    	

        }

        #endregion pin creation
        
        #region mainloop
        
        public void Configurate(IPluginConfig Input)
        {
        	//nothing to configure in this plugin
        	//only used in conjunction with inputs of type cmpdConfigurate
        }
        
        //here we go, thats the method called by vvvv each frame
        //all data handling should be in here
        public void Evaluate(int SpreadMax)
        {     	
        	//compute only on refresh	
        	if (FRefresh.PinIsChanged)
        	{
        		
        		System.Windows.Forms.Screen [] screens = System.Windows.Forms.Screen.AllScreens;
        		
        		//set slicecount
        		FResolutionOutput.SliceCount = screens.Length;
        		FResolutionOffsetOutput.SliceCount = screens.Length;
        		FWorkAreaOutput.SliceCount = screens.Length;
        		FWorkAreaOffsetOutput.SliceCount = screens.Length;
        		FIsPrimary.SliceCount = screens.Length;
	        	
        		//loop for all slices
        		for (int i=0; i<=screens.GetUpperBound(0); i++)
        		{		
        			//write data to outputs
        			FResolutionOutput.SetValue2D(i, screens[i].Bounds.Width, screens[i].Bounds.Height);
        			FResolutionOffsetOutput.SetValue2D(i, screens[i].Bounds.Left, screens[i].Bounds.Top);
        			FWorkAreaOutput.SetValue2D(i, screens[i].WorkingArea.Width, screens[i].WorkingArea.Height);
        			FWorkAreaOffsetOutput.SetValue2D(i, screens[i].WorkingArea.Left, screens[i].WorkingArea.Top);
        			FBitsPerPixel.SetValue(i, (double)screens[i].BitsPerPixel);
        			FIsPrimary.SetValue(i, (double)screens[i].Primary.GetHashCode());
        			
        		}
        	}
        	      	
        }
             
        #endregion mainloop  
	}
}
