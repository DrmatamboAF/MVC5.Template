using System.Web.Optimization;

namespace MvcTemplate.Web
{
    public interface IBundleConfig
    {
        void RegisterBundles(BundleCollection bundles);
    }
}
