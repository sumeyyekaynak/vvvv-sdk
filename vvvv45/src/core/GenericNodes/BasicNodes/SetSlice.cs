#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes.Generic
{

    public class SetSlice<T> : IPluginEvaluate, IPartImportsSatisfiedNotification
    {
        #region fields & pins
#pragma warning disable 0649
        protected IIOContainer<ISpread<ISpread<T>>> FSpreadContainer;
        protected IIOContainer<ISpread<T>> FInputContainer;

        [Input("Index", Order = 100)]
        ISpread<int> FIndex;

        protected IIOContainer<ISpread<ISpread<T>>> FOutputContainer;

        [Import]
        IIOFactory FFactory;
#pragma warning restore
        #endregion fields & pins

        public void OnImportsSatisfied()
        {
            FSpreadContainer = FFactory.CreateIOContainer<ISpread<ISpread<T>>>(
                new InputAttribute("Spread") { BinName = "Bin Size", BinSize = 1, BinOrder = 1 });

            FInputContainer = FFactory.CreateIOContainer<ISpread<T>>(
                new InputAttribute("Input"));

            FOutputContainer = FFactory.CreateIOContainer<ISpread<ISpread<T>>>(
                new OutputAttribute("Output"));
        }

        protected virtual void Prepare() { }

        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {
            Prepare();

            var count = FOutputContainer.IOObject.SliceCount = FSpreadContainer.IOObject.SliceCount;

            int incr = 0;
            for (int i = 0; i < count; i++)
            {
                var os = FOutputContainer.IOObject[i];
                var ind = VMath.Zmod(FIndex[i], count);
                if (i != ind)
                    os.AssignFrom(FSpreadContainer.IOObject[i]);
                else
                {
                    var osCount = os.SliceCount;
                    for (int s = 0; s < osCount; s++)
                        os[s] = FInputContainer.IOObject[incr + s];
                    incr += osCount;
                }
            }
        }
    }

}
