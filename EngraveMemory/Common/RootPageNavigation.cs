using Xamarin.Forms;
 
 namespace EngraveMemory.Common
 {
     public class RootPageNavigation
     {
         public INavigation Navigation { get; private set; }
 
         public void SetNavigation(INavigation navigation)
         {
             Navigation = navigation;
         }
 
     }
 }