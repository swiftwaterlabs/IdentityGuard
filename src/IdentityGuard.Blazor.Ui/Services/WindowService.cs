using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IWindowService
    {
        Task Open(string url);
    }

    public class WindowService : IWindowService
    {
        private IJSRuntime _jsRuntime;
        public WindowService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Opens up a link in new tab, also usage of eval is not harmful in this case, 
        /// source: https://stackoverflow.com/questions/197769/when-is-javascripts-eval-not-evil
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task Open(string url)
        {
            await _jsRuntime.InvokeVoidAsync("eval", $"let _discard_ = open(`{url}`, `_blank`)");
        }
    }
}
