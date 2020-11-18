/*!
 * jQuery Accordion 0.0.1
 * (c) 2014 Victor Fernandez <victor@vctrfrnndz.com>
 * MIT Licensed.
 */

;(function ( $, window, document, undefined ) {

    var pluginName = 'accordion',
        defaults = {
            transitionSpeed: 300,
            transitionEasing: 'ease',
            controlElement: '[data-control]',
            contentElement: '[data-content]',
            groupElement: '[data-accordion-group]',
            singleOpen: true
           
        };

    function Accordion(element, options) {
       
        this.element = element;
        this.options = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    Accordion.prototype.init = function () {
        var self = this,
            opts = self.options;
         var $accordion = $(self.element),
            $controls = $accordion.find('> ' + opts.controlElement),
            $content =  $accordion.find('> ' + opts.contentElement);

        var accordionParentsQty = $accordion.parents('[data-accordion]').length,
            accordionHasParent = accordionParentsQty > 0;

        var closedCSS = { 'max-height': 0, 'overflow': 'hidden' };

        var CSStransitions = supportsTransitions();

        function debounce(func, threshold, execAsap) {
            var timeout;

            return function debounced() {
                var obj = this,
                    args = arguments;

                function delayed() {
                    if (!execAsap) func.apply(obj, args);
                    timeout = null;
                };

                if (timeout) clearTimeout(timeout);
                else if (execAsap) func.apply(obj, args);

                timeout = setTimeout(delayed, threshold || 100);
            };
        }

        function supportsTransitions() {
            var b = document.body || document.documentElement,
                s = b.style,
                p = 'transition';

            if (typeof s[p] == 'string') {
                return true;
            }

            var v = ['Moz', 'webkit', 'Webkit', 'Khtml', 'O', 'ms'];

            p = 'Transition';

            for (var i=0; i<v.length; i++) {
                if (typeof s[v[i] + p] == 'string') {
                    return true;
                }
            }

            return false;
        }

        function requestAnimFrame(cb) {
            if(window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame) {
                return  requestAnimationFrame(cb) ||
                        webkitRequestAnimationFrame(cb) ||
                        mozRequestAnimationFrame(cb);
            } else {
                return setTimeout(cb, 1000 / 60);
            }
        }

        function toggleTransition($el, remove) {
            if(!remove) {
                $content.css({
                    '-webkit-transition': 'max-height ' + opts.transitionSpeed + 'ms ' + opts.transitionEasing,
                    'transition': 'max-height ' + opts.transitionSpeed + 'ms ' + opts.transitionEasing
                });
            } else {
                $content.css({
                    '-webkit-transition': '',
                    'transition': ''
                });
            }
        }

        function calculateHeight($el) {
            var height = 0;

            $el.children().each(function() {
                height = height + $(this).outerHeight(true);
            });

            $el.data('oHeight', height);
        }

        function updateParentHeight($parentAccordion, $currentAccordion, qty, operation) {
            var $content = $parentAccordion.filter('.open').find('> [data-content]'),
                $childs = $content.find('[data-accordion].open > [data-content]'),
				$childs = $content.find('[data-accordion].open > [button]'),
                $matched;

            if(!opts.singleOpen) {
                $childs = $childs.not($currentAccordion.siblings('[data-accordion].open').find('> [data-content]'));
            }

            $matched = $content.add($childs);

            if($parentAccordion.hasClass('open')) {
                $matched.each(function() {
                    var currentHeight = $(this).data('oHeight');

                    switch (operation) {
                        case '+':
                            $(this).data('oHeight', currentHeight + qty);
                            break;
                        case '-':
                            $(this).data('oHeight', currentHeight - qty);
                            break;
                        default:
                            throw 'updateParentHeight method needs an operation';
                    }

                    $(this).css('max-height', $(this).data('oHeight'));
                });
            }
        }

        function refreshHeight($accordion) {
            if($accordion.hasClass('open')) {
                var $content = $accordion.find('> [data-content]'),
                    $childs = $content.find('[data-accordion].open > [data-content]'),
                    $matched = $content.add($childs);

                calculateHeight($matched);

                $matched.css('max-height', $matched.data('oHeight'));
            }
        }

        function closeAccordion($accordion, $content) {
            $accordion.trigger('accordion.close');
            
            if(CSStransitions) {
                if(accordionHasParent) {
                    var $parentAccordions = $accordion.parents('[data-accordion]');

                    updateParentHeight($parentAccordions, $accordion, $content.data('oHeight'), '-');
                }

                $content.css(closedCSS);

                $accordion.removeClass('open');
            } else {
                $content.css('max-height', $content.data('oHeight'));

                $content.animate(closedCSS, opts.transitionSpeed);

                $accordion.removeClass('open');
            }
        }

        function openAccordion($accordion, $content) {
            $accordion.trigger('accordion.open');
            if(CSStransitions) {
                toggleTransition($content);

                if(accordionHasParent) {
                    var $parentAccordions = $accordion.parents('[data-accordion]');

                    updateParentHeight($parentAccordions, $accordion, $content.data('oHeight'), '+');
                }

                requestAnimFrame(function() {
                    $content.css('max-height', $content.data('oHeight'));
                });

                $accordion.addClass('open');
            } else {
                $content.animate({
                    'max-height': $content.data('oHeight')
                }, opts.transitionSpeed, function() {
                    $content.css({'max-height': 'none'});
                });

                $accordion.addClass('open');
            }
        }

        function closeSiblingAccordions($accordion) {
            var $accordionGroup = $accordion.closest(opts.groupElement);

            var $siblings = $accordion.siblings('[data-accordion]').filter('.open'),
                $siblingsChildren = $siblings.find('[data-accordion]').filter('.open');

            var $otherAccordions = $siblings.add($siblingsChildren);

            $otherAccordions.each(function() {
                var $accordion = $(this),
                    $content = $accordion.find(opts.contentElement);

                closeAccordion($accordion, $content);
            });

            $otherAccordions.removeClass('open');
        }

        function toggleAccordion() {
            var isAccordionGroup = (opts.singleOpen) ? $accordion.parents(opts.groupElement).length > 0 : false;

            calculateHeight($content);

            if(isAccordionGroup) {
                closeSiblingAccordions($accordion);
            }

            if($accordion.hasClass('open')) {
                closeAccordion($accordion, $content);
            } else {
                openAccordion($accordion, $content);
            }
        }

        function addEventListeners() {
            $controls.on('click', toggleAccordion);
            
            $controls.on('accordion.toggle', function() {
                if(opts.singleOpen && $controls.length > 1) {
                    return false;
                }
                
                toggleAccordion();
            });

            $(window).on('resize', debounce(function() {
                refreshHeight($accordion);
            }));
        }

        function setup() {
            $content.each(function() {
                var $curr = $(this);
         
                if($curr.css('max-height') != 0) {
                    if(!$curr.closest('[data-accordion]').hasClass('open')) {
                        $curr.css({ 'max-height': 0, 'overflow': 'hidden' });
                    } else {
                        toggleTransition($curr);
                        calculateHeight($curr);

                        $curr.css('max-height', $curr.data('oHeight'));
                    }
                }
            });


            if(!$accordion.attr('data-accordion')) {
                $accordion.attr('data-accordion', '');
                $accordion.find(opts.controlElement).attr('data-control', '');
                $accordion.find(opts.contentElement).attr('data-content', '');
            }
        }

        setup();
        addEventListeners();
    };

    $.fn[pluginName] = function ( options ) {
        return this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName)) {
                $.data(this, 'plugin_' + pluginName,
                new Accordion( this, options ));
            }
        });
    }

})( jQuery, window, document );



/*** include jquery.customSelect.js ***/


/*!
* jquery.customSelect() - v0.5.1
* http://adam.co/lab/jquery/customselect/
* 2014-03-19
*
* Copyright 2013 Adam Coulombe
* @license http://www.opensource.org/licenses/mit-license.html MIT License
* @license http://www.gnu.org/licenses/gpl.html GPL2 License 
*/

(function ($) {
    'use strict';

    $.fn.extend({
        customSelect: function (options) {
            // filter out <= IE6
            if (typeof document.body.style.maxHeight === 'undefined') {
                return this;
            }
            var defaults = {
                customClass: 'customSelect',
                mapClass: true,
                mapStyle: true
            },
            options = $.extend(defaults, options),
            prefix = options.customClass,
            changed = function ($select, customSelectSpan) {
                var currentSelected = $select.find(':selected'),
                customSelectSpanInner = customSelectSpan.children(':first'),
                html = currentSelected.html() || '&nbsp;';

                customSelectSpanInner.html(html);

                if (currentSelected.attr('disabled')) {
                    customSelectSpan.addClass(getClass('DisabledOption'));
                } else {
                    customSelectSpan.removeClass(getClass('DisabledOption'));
                }

                setTimeout(function () {
                    customSelectSpan.removeClass(getClass('Open'));
                    $(document).off('mouseup.customSelect');
                }, 60);
            },
            getClass = function (suffix) {
                return prefix + suffix;
            };

            return this.each(function () {
                var $select = $(this),
                    customSelectInnerSpan = $('<span />').addClass(getClass('Inner')),
                    customSelectSpan = $('<span />');

                $select.after(customSelectSpan.append(customSelectInnerSpan));

                customSelectSpan.addClass(prefix);

                if (options.mapClass) {
                    customSelectSpan.addClass($select.attr('class'));
                }
                if (options.mapStyle) {
                    customSelectSpan.attr('style', $select.attr('style'));
                }

                $select
                    .addClass('hasCustomSelect')
                    .on('render.customSelect', function () {
                        changed($select, customSelectSpan);
                        $select.css('width', '');
                        var selectBoxWidth = parseInt($select.outerWidth(), 10) -
                                (parseInt(customSelectSpan.outerWidth(), 10) -
                                    parseInt(customSelectSpan.width(), 10));

                        // Set to inline-block before calculating outerHeight
                        customSelectSpan.css({
                            display: 'inline-block'
                        });

                        var selectBoxHeight = customSelectSpan.outerHeight();

                        if ($select.attr('disabled')) {
                            customSelectSpan.addClass(getClass('Disabled'));
                        } else {
                            customSelectSpan.removeClass(getClass('Disabled'));
                        }

                        customSelectInnerSpan.css({
                            width: selectBoxWidth,
                            display: 'inline-block'
                        });

                        $select.css({
                            '-webkit-appearance': 'menulist-button',
                            width: customSelectSpan.outerWidth(),
                            position: 'absolute',
                            opacity: 0,
                            height: selectBoxHeight,
                            fontSize: customSelectSpan.css('font-size')
                        });
                    })
                    .on('change.customSelect', function () {
                        customSelectSpan.addClass(getClass('Changed'));
                        changed($select, customSelectSpan);
                    })
                    .on('keyup.customSelect', function (e) {
                        if (!customSelectSpan.hasClass(getClass('Open'))) {
                            $select.trigger('blur.customSelect');
                            $select.trigger('focus.customSelect');
                        } else {
                            if (e.which == 13 || e.which == 27) {
                                changed($select, customSelectSpan);
                            }
                        }
                    })
                    .on('mousedown.customSelect', function () {
                        customSelectSpan.removeClass(getClass('Changed'));
                    })
                    .on('mouseup.customSelect', function (e) {

                        if (!customSelectSpan.hasClass(getClass('Open'))) {
                            // if FF and there are other selects open, just apply focus
                            if ($('.' + getClass('Open')).not(customSelectSpan).length > 0 && typeof InstallTrigger !== 'undefined') {
                                $select.trigger('focus.customSelect');
                            } else {
                                customSelectSpan.addClass(getClass('Open'));
                                e.stopPropagation();
                                $(document).one('mouseup.customSelect', function (e) {
                                    if (e.target != $select.get(0) && $.inArray(e.target, $select.find('*').get()) < 0) {
                                        $select.trigger('blur.customSelect');
                                    } else {
                                        changed($select, customSelectSpan);
                                    }
                                });
                            }
                        }
                    })
                    .on('focus.customSelect', function () {
                        customSelectSpan.removeClass(getClass('Changed')).addClass(getClass('Focus'));
                    })
                    .on('blur.customSelect', function () {
                        customSelectSpan.removeClass(getClass('Focus') + ' ' + getClass('Open'));
                    })
                    .on('mouseenter.customSelect', function () {
                        customSelectSpan.addClass(getClass('Hover'));
                    })
                    .on('mouseleave.customSelect', function () {
                        customSelectSpan.removeClass(getClass('Hover'));
                    })
                    .trigger('render.customSelect');
            });
        }
    });
})(jQuery);



    /*** include lazyload ***/



    (function ($, window, document, undefined) { var $window = $(window); $.fn.lazyload = function (options) { var elements = this; var $container; var settings = { threshold: 0, failure_limit: 0, event: "scroll", effect: "show", container: window, data_attribute: "original", skip_invisible: true, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; function update() { var counter = 0; elements.each(function () { var $this = $(this); if (settings.skip_invisible && !$this.is(":visible")) { return } if ($.abovethetop(this, settings) || $.leftofbegin(this, settings)) { } else { if (!$.belowthefold(this, settings) && !$.rightoffold(this, settings)) { $this.trigger("appear"); counter = 0 } else { if (++counter > settings.failure_limit) { return false } } } }) } if (options) { if (undefined !== options.failurelimit) { options.failure_limit = options.failurelimit; delete options.failurelimit } if (undefined !== options.effectspeed) { options.effect_speed = options.effectspeed; delete options.effectspeed } $.extend(settings, options) } $container = (settings.container === undefined || settings.container === window) ? $window : $(settings.container); if (0 === settings.event.indexOf("scroll")) { $container.bind(settings.event, function () { return update() }) } this.each(function () { var self = this; var $self = $(self); self.loaded = false; if ($self.attr("src") === undefined || $self.attr("src") === false) { if ($self.is("img")) { $self.attr("src", settings.placeholder) } } $self.one("appear", function () { if (!this.loaded) { if (settings.appear) { var elements_left = elements.length; settings.appear.call(self, elements_left, settings) } $("<img />").bind("load", function () { var original = $self.attr("data-" + settings.data_attribute); $self.hide(); if ($self.is("img")) { $self.attr("src", original) } else { $self.css("background-image", "url('" + original + "')") } $self[settings.effect](settings.effect_speed); self.loaded = true; var temp = $.grep(elements, function (element) { return !element.loaded }); elements = $(temp); if (settings.load) { var elements_left = elements.length; settings.load.call(self, elements_left, settings) } }).attr("src", $self.attr("data-" + settings.data_attribute)) } }); if (0 !== settings.event.indexOf("scroll")) { $self.bind(settings.event, function () { if (!self.loaded) { $self.trigger("appear") } }) } }); $window.bind("resize", function () { update() }); if ((/(?:iphone|ipod|ipad).*os 5/gi).test(navigator.appVersion)) { $window.bind("pageshow", function (event) { if (event.originalEvent && event.originalEvent.persisted) { elements.each(function () { $(this).trigger("appear") }) } }) } $(document).ready(function () { update() }); return this }; $.belowthefold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = (window.innerHeight ? window.innerHeight : $window.height()) + $window.scrollTop() } else { fold = $(settings.container).offset().top + $(settings.container).height() } return fold <= $(element).offset().top - settings.threshold }; $.rightoffold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.width() + $window.scrollLeft() } else { fold = $(settings.container).offset().left + $(settings.container).width() } return fold <= $(element).offset().left - settings.threshold }; $.abovethetop = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollTop() } else { fold = $(settings.container).offset().top } return fold >= $(element).offset().top + settings.threshold + $(element).height() }; $.leftofbegin = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollLeft() } else { fold = $(settings.container).offset().left } return fold >= $(element).offset().left + settings.threshold + $(element).width() }; $.inviewport = function (element, settings) { return !$.rightoffold(element, settings) && !$.leftofbegin(element, settings) && !$.belowthefold(element, settings) && !$.abovethetop(element, settings) }; $.extend($.expr[":"], { "below-the-fold": function (a) { return $.belowthefold(a, { threshold: 0 }) }, "above-the-top": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-screen": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-screen": function (a) { return !$.rightoffold(a, { threshold: 0 }) }, "in-viewport": function (a) { return $.inviewport(a, { threshold: 0 }) }, "above-the-fold": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-fold": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-fold": function (a) { return !$.rightoffold(a, { threshold: 0 }) } }) })(jQuery, window, document);

