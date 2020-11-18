/*
 **
 * multiaccordion.jquery.js
 * Pierre SKOWRON - 2014-11-04
 *
 **
 * Dependencies
 * - jQuery
 * - jQuery UI
 **
 * HowTo
 * $(selector).multiaccordion(options);
 **
 * Options (Object) - facultative
 * - header (String) : selector for the header
 * - container (String) : selector for the container
 * - defaultIcon (String) : jQuery UI icon when accordion is closed
 * - activeIcon (String) : jQuery UI icon when accordion is open
*/

jQuery.fn.multiaccordion = function(options) {
  var defaults = {
    header: "h3",
    container: ".content",
    defaultIcon: "ui-icon-circle-arrow-e",
    activeIcon: "ui-icon-circle-arrow-s"
  };

  if (this.length > 0) {
    return jQuery(this).each(function(i) {
      var opts = $.extend(defaults, options),
      $this = $(this);

      $this.addClass("ui-accordion ui-widget")
        .children(opts.header + ":first-child")
        .off()
        .addClass("ui-accordion-header ui-state-active ui-accordion-header-active")
        .prepend('<span class="ui-icon ui-accordion-header-icon ' + opts.activeIcon + '"></span>')
        .hover(function() {
          $(this).toggleClass("ui-state-hover");
        })
        .click(function() {
          $(this).toggleClass("ui-accordion-header-active ui-state-active ui-state-default")
            .find("> .ui-icon").toggleClass(opts.defaultIcon + " " + opts.activeIcon)
            .end()
            .next(opts.container)
            .toggleClass("ui-accordion-content-active")
            .off()
            .slideToggle();
          })
        .next(opts.container)
        .addClass("ui-widget-content ui-accordion-content-active");
      });
    }
};
