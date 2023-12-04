using System.Collections.Generic;
using System.Linq;
using Microsoft.Playwright;

namespace Coypu.Drivers.Playwright
{
    internal class FrameFinder
    {
        private readonly IPage _page;
        private readonly XPath _xPath;

        public FrameFinder(IPage page)
        {
            _page = page;
            _xPath = new XPath();
        }

        public IEnumerable<PlaywrightFrame> FindFrame(string locator,
                                                  IEnumerable<IElementHandle> frameElements,
                                                  Options options)
        {
            return PlaywrightFrames(locator, frameElements, options);
        }

        private IEnumerable<PlaywrightFrame> PlaywrightFrames(string locator,
                                                       IEnumerable<IElementHandle> frameElements,
                                                       Options options)
        {
            var allPlaywrightFrames = frameElements.Select(f => new PlaywrightFrame(f));
            return allPlaywrightFrames.Where(f => {
                var t = f.Title;
                return f.Id == locator ||
                                             f.Name == locator ||
                                             (options.TextPrecisionExact
                                               ? f.Title == locator
                                               : f.Title
                                                  .Contains(locator)) ||
                                          FrameContentsMatch(f, locator, options);
            });
        }

        private bool FrameContentsMatch(PlaywrightFrame frame,
                                        string locator,
                                        Options options)
        {
            return frame.Title == locator ||
                    Async.WaitForResult(
                        ((IFrame) frame.Native).QuerySelectorAllAsync(
                            $"xpath=.//h1[{_xPath.IsText(locator, options)}]"
                        )
                    ).Any();
        }
    }
}
