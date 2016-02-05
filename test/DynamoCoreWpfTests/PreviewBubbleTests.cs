using System.Collections.Generic;
using System.Windows;
using DynamoCoreWpfTests.Utility;
using NUnit.Framework;

namespace DynamoCoreWpfTests
{
    class PreviewBubbleTests : DynamoTestUIBase
    {
        protected override void GetLibrariesToPreload(List<string> libraries)
        {
            libraries.Add("DSCoreNodes.dll");
            base.GetLibrariesToPreload(libraries);
        }

        public override void Open(string path)
        {
            base.Open(path);

            DispatcherUtil.DoEvents();
        }

        public override void Run()
        {
            base.Run();

            DispatcherUtil.DoEvents();
        }

        [Test]
        public void PreviewBubble_HiddenDummyVerticalBoundaries()
        {
            Open(@"core\DetailedPreviewMargin_Test.dyn");

            var nodeView = NodeViewWithGuid("aeb8734f-d782-4c19-9648-9215da485342");
            nodeView.PreviewControl.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
                        
            // preview is hidden
            Assert.IsTrue(ElementIsInContainer(nodeView.PreviewControl.HiddenDummy, nodeView.PreviewControl));

            View.Dispatcher.Invoke(() =>
            {
                nodeView.PreviewControl.BindToDataSource(nodeView.ViewModel.NodeModel.CachedValue);
                nodeView.PreviewControl.TransitionToState(Dynamo.UI.Controls.PreviewControl.State.Condensed);
            });

            DispatcherUtil.DoEvents();

            // preview is condensed
            Assert.IsTrue(ElementIsInContainer(nodeView.PreviewControl.HiddenDummy, nodeView.PreviewControl));

            View.Dispatcher.Invoke(() =>
            {
                nodeView.PreviewControl.TransitionToState(Dynamo.UI.Controls.PreviewControl.State.Expanded);
            });

            DispatcherUtil.DoEvents();

            // preview is expanded
            Assert.IsTrue(ElementIsInContainer(nodeView.PreviewControl.HiddenDummy, nodeView));
        }        

        private bool ElementIsInContainer(FrameworkElement element, FrameworkElement container)
        {
            var relativePosition = element.TranslatePoint(new Point(), container);
            
            return (relativePosition.X == 0) && (element.ActualWidth <= container.ActualWidth);
        }
    }
}
