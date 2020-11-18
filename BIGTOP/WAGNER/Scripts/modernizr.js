


/* Include bootstrab.min.js */


/*!
* Bootstrap v3.3.4 (http://getbootstrap.com)
* Copyright 2011-2015 Twitter, Inc.
* Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
*/

/*!
* Generated using the Bootstrap Customizer (http://getbootstrap.com/customize/?id=9871813fd612ac52f05d)
* Config saved to config.json and https://gist.github.com/9871813fd612ac52f05d
*/
if ("undefined" == typeof jQuery) throw new Error("Bootstrap's JavaScript requires jQuery"); +function (t) { "use strict"; var e = t.fn.jquery.split(" ")[0].split("."); if (e[0] < 2 && e[1] < 9 || 1 == e[0] && 9 == e[1] && e[2] < 1) throw new Error("Bootstrap's JavaScript requires jQuery version 1.9.1 or higher") } (jQuery), +function (t) { "use strict"; function e(e) { return this.each(function () { var i = t(this), n = i.data("bs.alert"); n || i.data("bs.alert", n = new o(this)), "string" == typeof e && n[e].call(i) }) } var i = '[data-dismiss="alert"]', o = function (e) { t(e).on("click", i, this.close) }; o.VERSION = "3.3.4", o.TRANSITION_DURATION = 150, o.prototype.close = function (e) { function i() { a.detach().trigger("closed.bs.alert").remove() } var n = t(this), s = n.attr("data-target"); s || (s = n.attr("href"), s = s && s.replace(/.*(?=#[^\s]*$)/, "")); var a = t(s); e && e.preventDefault(), a.length || (a = n.closest(".alert")), a.trigger(e = t.Event("close.bs.alert")), e.isDefaultPrevented() || (a.removeClass("in"), t.support.transition && a.hasClass("fade") ? a.one("bsTransitionEnd", i).emulateTransitionEnd(o.TRANSITION_DURATION) : i()) }; var n = t.fn.alert; t.fn.alert = e, t.fn.alert.Constructor = o, t.fn.alert.noConflict = function () { return t.fn.alert = n, this }, t(document).on("click.bs.alert.data-api", i, o.prototype.close) } (jQuery), +function (t) { "use strict"; function e(e) { return this.each(function () { var o = t(this), n = o.data("bs.button"), s = "object" == typeof e && e; n || o.data("bs.button", n = new i(this, s)), "toggle" == e ? n.toggle() : e && n.setState(e) }) } var i = function (e, o) { this.$element = t(e), this.options = t.extend({}, i.DEFAULTS, o), this.isLoading = !1 }; i.VERSION = "3.3.4", i.DEFAULTS = { loadingText: "loading..." }, i.prototype.setState = function (e) { var i = "disabled", o = this.$element, n = o.is("input") ? "val" : "html", s = o.data(); e += "Text", null == s.resetText && o.data("resetText", o[n]()), setTimeout(t.proxy(function () { o[n](null == s[e] ? this.options[e] : s[e]), "loadingText" == e ? (this.isLoading = !0, o.addClass(i).attr(i, i)) : this.isLoading && (this.isLoading = !1, o.removeClass(i).removeAttr(i)) }, this), 0) }, i.prototype.toggle = function () { var t = !0, e = this.$element.closest('[data-toggle="buttons"]'); if (e.length) { var i = this.$element.find("input"); "radio" == i.prop("type") && (i.prop("checked") && this.$element.hasClass("active") ? t = !1 : e.find(".active").removeClass("active")), t && i.prop("checked", !this.$element.hasClass("active")).trigger("change") } else this.$element.attr("aria-pressed", !this.$element.hasClass("active")); t && this.$element.toggleClass("active") }; var o = t.fn.button; t.fn.button = e, t.fn.button.Constructor = i, t.fn.button.noConflict = function () { return t.fn.button = o, this }, t(document).on("click.bs.button.data-api", '[data-toggle^="button"]', function (i) { var o = t(i.target); o.hasClass("btn") || (o = o.closest(".btn")), e.call(o, "toggle"), i.preventDefault() }).on("focus.bs.button.data-api blur.bs.button.data-api", '[data-toggle^="button"]', function (e) { t(e.target).closest(".btn").toggleClass("focus", /^focus(in)?$/.test(e.type)) }) } (jQuery), +function (t) { "use strict"; function e(e) { return this.each(function () { var o = t(this), n = o.data("bs.carousel"), s = t.extend({}, i.DEFAULTS, o.data(), "object" == typeof e && e), a = "string" == typeof e ? e : s.slide; n || o.data("bs.carousel", n = new i(this, s)), "number" == typeof e ? n.to(e) : a ? n[a]() : s.interval && n.pause().cycle() }) } var i = function (e, i) { this.$element = t(e), this.$indicators = this.$element.find(".carousel-indicators"), this.options = i, this.paused = null, this.sliding = null, this.interval = null, this.$active = null, this.$items = null, this.options.keyboard && this.$element.on("keydown.bs.carousel", t.proxy(this.keydown, this)), "hover" == this.options.pause && !("ontouchstart" in document.documentElement) && this.$element.on("mouseenter.bs.carousel", t.proxy(this.pause, this)).on("mouseleave.bs.carousel", t.proxy(this.cycle, this)) }; i.VERSION = "3.3.4", i.TRANSITION_DURATION = 600, i.DEFAULTS = { interval: 5e3, pause: "hover", wrap: !0, keyboard: !0 }, i.prototype.keydown = function (t) { if (!/input|textarea/i.test(t.target.tagName)) { switch (t.which) { case 37: this.prev(); break; case 39: this.next(); break; default: return } t.preventDefault() } }, i.prototype.cycle = function (e) { return e || (this.paused = !1), this.interval && clearInterval(this.interval), this.options.interval && !this.paused && (this.interval = setInterval(t.proxy(this.next, this), this.options.interval)), this }, i.prototype.getItemIndex = function (t) { return this.$items = t.parent().children(".item"), this.$items.index(t || this.$active) }, i.prototype.getItemForDirection = function (t, e) { var i = this.getItemIndex(e), o = "prev" == t && 0 === i || "next" == t && i == this.$items.length - 1; if (o && !this.options.wrap) return e; var n = "prev" == t ? -1 : 1, s = (i + n) % this.$items.length; return this.$items.eq(s) }, i.prototype.to = function (t) { var e = this, i = this.getItemIndex(this.$active = this.$element.find(".item.active")); return t > this.$items.length - 1 || 0 > t ? void 0 : this.sliding ? this.$element.one("slid.bs.carousel", function () { e.to(t) }) : i == t ? this.pause().cycle() : this.slide(t > i ? "next" : "prev", this.$items.eq(t)) }, i.prototype.pause = function (e) { return e || (this.paused = !0), this.$element.find(".next, .prev").length && t.support.transition && (this.$element.trigger(t.support.transition.end), this.cycle(!0)), this.interval = clearInterval(this.interval), this }, i.prototype.next = function () { return this.sliding ? void 0 : this.slide("next") }, i.prototype.prev = function () { return this.sliding ? void 0 : this.slide("prev") }, i.prototype.slide = function (e, o) { var n = this.$element.find(".item.active"), s = o || this.getItemForDirection(e, n), a = this.interval, r = "next" == e ? "left" : "right", l = this; if (s.hasClass("active")) return this.sliding = !1; var h = s[0], d = t.Event("slide.bs.carousel", { relatedTarget: h, direction: r }); if (this.$element.trigger(d), !d.isDefaultPrevented()) { if (this.sliding = !0, a && this.pause(), this.$indicators.length) { this.$indicators.find(".active").removeClass("active"); var p = t(this.$indicators.children()[this.getItemIndex(s)]); p && p.addClass("active") } var c = t.Event("slid.bs.carousel", { relatedTarget: h, direction: r }); return t.support.transition && this.$element.hasClass("slide") ? (s.addClass(e), s[0].offsetWidth, n.addClass(r), s.addClass(r), n.one("bsTransitionEnd", function () { s.removeClass([e, r].join(" ")).addClass("active"), n.removeClass(["active", r].join(" ")), l.sliding = !1, setTimeout(function () { l.$element.trigger(c) }, 0) }).emulateTransitionEnd(i.TRANSITION_DURATION)) : (n.removeClass("active"), s.addClass("active"), this.sliding = !1, this.$element.trigger(c)), a && this.cycle(), this } }; var o = t.fn.carousel; t.fn.carousel = e, t.fn.carousel.Constructor = i, t.fn.carousel.noConflict = function () { return t.fn.carousel = o, this }; var n = function (i) { var o, n = t(this), s = t(n.attr("data-target") || (o = n.attr("href")) && o.replace(/.*(?=#[^\s]+$)/, "")); if (s.hasClass("carousel")) { var a = t.extend({}, s.data(), n.data()), r = n.attr("data-slide-to"); r && (a.interval = !1), e.call(s, a), r && s.data("bs.carousel").to(r), i.preventDefault() } }; t(document).on("click.bs.carousel.data-api", "[data-slide]", n).on("click.bs.carousel.data-api", "[data-slide-to]", n), t(window).on("load", function () { t('[data-ride="carousel"]').each(function () { var i = t(this); e.call(i, i.data()) }) }) } (jQuery), +function (t) { "use strict"; function e(e) { e && 3 === e.which || (t(n).remove(), t(s).each(function () { var o = t(this), n = i(o), s = { relatedTarget: this }; n.hasClass("open") && (n.trigger(e = t.Event("hide.bs.dropdown", s)), e.isDefaultPrevented() || (o.attr("aria-expanded", "false"), n.removeClass("open").trigger("hidden.bs.dropdown", s))) })) } function i(e) { var i = e.attr("data-target"); i || (i = e.attr("href"), i = i && /#[A-Za-z]/.test(i) && i.replace(/.*(?=#[^\s]*$)/, "")); var o = i && t(i); return o && o.length ? o : e.parent() } function o(e) { return this.each(function () { var i = t(this), o = i.data("bs.dropdown"); o || i.data("bs.dropdown", o = new a(this)), "string" == typeof e && o[e].call(i) }) } var n = ".dropdown-backdrop", s = '[data-toggle="dropdown"]', a = function (e) { t(e).on("click.bs.dropdown", this.toggle) }; a.VERSION = "3.3.4", a.prototype.toggle = function (o) { var n = t(this); if (!n.is(".disabled, :disabled")) { var s = i(n), a = s.hasClass("open"); if (e(), !a) { "ontouchstart" in document.documentElement && !s.closest(".navbar-nav").length && t('<div class="dropdown-backdrop"/>').insertAfter(t(this)).on("click", e); var r = { relatedTarget: this }; if (s.trigger(o = t.Event("show.bs.dropdown", r)), o.isDefaultPrevented()) return; n.trigger("focus").attr("aria-expanded", "true"), s.toggleClass("open").trigger("shown.bs.dropdown", r) } return !1 } }, a.prototype.keydown = function (e) { if (/(38|40|27|32)/.test(e.which) && !/input|textarea/i.test(e.target.tagName)) { var o = t(this); if (e.preventDefault(), e.stopPropagation(), !o.is(".disabled, :disabled")) { var n = i(o), a = n.hasClass("open"); if (!a && 27 != e.which || a && 27 == e.which) return 27 == e.which && n.find(s).trigger("focus"), o.trigger("click"); var r = " li:not(.disabled):visible a", l = n.find('[role="menu"]' + r + ', [role="listbox"]' + r); if (l.length) { var h = l.index(e.target); 38 == e.which && h > 0 && h--, 40 == e.which && h < l.length - 1 && h++, ~h || (h = 0), l.eq(h).trigger("focus") } } } }; var r = t.fn.dropdown; t.fn.dropdown = o, t.fn.dropdown.Constructor = a, t.fn.dropdown.noConflict = function () { return t.fn.dropdown = r, this }, t(document).on("click.bs.dropdown.data-api", e).on("click.bs.dropdown.data-api", ".dropdown form", function (t) { t.stopPropagation() }).on("click.bs.dropdown.data-api", s, a.prototype.toggle).on("keydown.bs.dropdown.data-api", s, a.prototype.keydown).on("keydown.bs.dropdown.data-api", '[role="menu"]', a.prototype.keydown).on("keydown.bs.dropdown.data-api", '[role="listbox"]', a.prototype.keydown) } (jQuery), +function (t) { "use strict"; function e(e, o) { return this.each(function () { var n = t(this), s = n.data("bs.modal"), a = t.extend({}, i.DEFAULTS, n.data(), "object" == typeof e && e); s || n.data("bs.modal", s = new i(this, a)), "string" == typeof e ? s[e](o) : a.show && s.show(o) }) } var i = function (e, i) { this.options = i, this.$body = t(document.body), this.$element = t(e), this.$dialog = this.$element.find(".modal-dialog"), this.$backdrop = null, this.isShown = null, this.originalBodyPad = null, this.scrollbarWidth = 0, this.ignoreBackdropClick = !1, this.options.remote && this.$element.find(".modal-content").load(this.options.remote, t.proxy(function () { this.$element.trigger("loaded.bs.modal") }, this)) }; i.VERSION = "3.3.4", i.TRANSITION_DURATION = 300, i.BACKDROP_TRANSITION_DURATION = 150, i.DEFAULTS = { backdrop: !0, keyboard: !0, show: !0 }, i.prototype.toggle = function (t) { return this.isShown ? this.hide() : this.show(t) }, i.prototype.show = function (e) { var o = this, n = t.Event("show.bs.modal", { relatedTarget: e }); this.$element.trigger(n), this.isShown || n.isDefaultPrevented() || (this.isShown = !0, this.checkScrollbar(), this.setScrollbar(), this.$body.addClass("modal-open"), this.escape(), this.resize(), this.$element.on("click.dismiss.bs.modal", '[data-dismiss="modal"]', t.proxy(this.hide, this)), this.$dialog.on("mousedown.dismiss.bs.modal", function () { o.$element.one("mouseup.dismiss.bs.modal", function (e) { t(e.target).is(o.$element) && (o.ignoreBackdropClick = !0) }) }), this.backdrop(function () { var n = t.support.transition && o.$element.hasClass("fade"); o.$element.parent().length || o.$element.appendTo(o.$body), o.$element.show().scrollTop(0), o.adjustDialog(), n && o.$element[0].offsetWidth, o.$element.addClass("in").attr("aria-hidden", !1), o.enforceFocus(); var s = t.Event("shown.bs.modal", { relatedTarget: e }); n ? o.$dialog.one("bsTransitionEnd", function () { o.$element.trigger("focus").trigger(s) }).emulateTransitionEnd(i.TRANSITION_DURATION) : o.$element.trigger("focus").trigger(s) })) }, i.prototype.hide = function (e) { e && e.preventDefault(), e = t.Event("hide.bs.modal"), this.$element.trigger(e), this.isShown && !e.isDefaultPrevented() && (this.isShown = !1, this.escape(), this.resize(), t(document).off("focusin.bs.modal"), this.$element.removeClass("in").attr("aria-hidden", !0).off("click.dismiss.bs.modal").off("mouseup.dismiss.bs.modal"), this.$dialog.off("mousedown.dismiss.bs.modal"), t.support.transition && this.$element.hasClass("fade") ? this.$element.one("bsTransitionEnd", t.proxy(this.hideModal, this)).emulateTransitionEnd(i.TRANSITION_DURATION) : this.hideModal()) }, i.prototype.enforceFocus = function () { t(document).off("focusin.bs.modal").on("focusin.bs.modal", t.proxy(function (t) { this.$element[0] === t.target || this.$element.has(t.target).length || this.$element.trigger("focus") }, this)) }, i.prototype.escape = function () { this.isShown && this.options.keyboard ? this.$element.on("keydown.dismiss.bs.modal", t.proxy(function (t) { 27 == t.which && this.hide() }, this)) : this.isShown || this.$element.off("keydown.dismiss.bs.modal") }, i.prototype.resize = function () { this.isShown ? t(window).on("resize.bs.modal", t.proxy(this.handleUpdate, this)) : t(window).off("resize.bs.modal") }, i.prototype.hideModal = function () { var t = this; this.$element.hide(), this.backdrop(function () { t.$body.removeClass("modal-open"), t.resetAdjustments(), t.resetScrollbar(), t.$element.trigger("hidden.bs.modal") }) }, i.prototype.removeBackdrop = function () { this.$backdrop && this.$backdrop.remove(), this.$backdrop = null }, i.prototype.backdrop = function (e) { var o = this, n = this.$element.hasClass("fade") ? "fade" : ""; if (this.isShown && this.options.backdrop) { var s = t.support.transition && n; if (this.$backdrop = t('<div class="modal-backdrop ' + n + '" />').appendTo(this.$body), this.$element.on("click.dismiss.bs.modal", t.proxy(function (t) { return this.ignoreBackdropClick ? void (this.ignoreBackdropClick = !1) : void (t.target === t.currentTarget && ("static" == this.options.backdrop ? this.$element[0].focus() : this.hide())) }, this)), s && this.$backdrop[0].offsetWidth, this.$backdrop.addClass("in"), !e) return; s ? this.$backdrop.one("bsTransitionEnd", e).emulateTransitionEnd(i.BACKDROP_TRANSITION_DURATION) : e() } else if (!this.isShown && this.$backdrop) { this.$backdrop.removeClass("in"); var a = function () { o.removeBackdrop(), e && e() }; t.support.transition && this.$element.hasClass("fade") ? this.$backdrop.one("bsTransitionEnd", a).emulateTransitionEnd(i.BACKDROP_TRANSITION_DURATION) : a() } else e && e() }, i.prototype.handleUpdate = function () { this.adjustDialog() }, i.prototype.adjustDialog = function () { var t = this.$element[0].scrollHeight > document.documentElement.clientHeight; this.$element.css({ paddingLeft: !this.bodyIsOverflowing && t ? this.scrollbarWidth : "", paddingRight: this.bodyIsOverflowing && !t ? this.scrollbarWidth : "" }) }, i.prototype.resetAdjustments = function () { this.$element.css({ paddingLeft: "", paddingRight: "" }) }, i.prototype.checkScrollbar = function () { var t = window.innerWidth; if (!t) { var e = document.documentElement.getBoundingClientRect(); t = e.right - Math.abs(e.left) } this.bodyIsOverflowing = document.body.clientWidth < t, this.scrollbarWidth = this.measureScrollbar() }, i.prototype.setScrollbar = function () { var t = parseInt(this.$body.css("padding-right") || 0, 10); this.originalBodyPad = document.body.style.paddingRight || "", this.bodyIsOverflowing && this.$body.css("padding-right", t + this.scrollbarWidth) }, i.prototype.resetScrollbar = function () { this.$body.css("padding-right", this.originalBodyPad) }, i.prototype.measureScrollbar = function () { var t = document.createElement("div"); t.className = "modal-scrollbar-measure", this.$body.append(t); var e = t.offsetWidth - t.clientWidth; return this.$body[0].removeChild(t), e }; var o = t.fn.modal; t.fn.modal = e, t.fn.modal.Constructor = i, t.fn.modal.noConflict = function () { return t.fn.modal = o, this }, t(document).on("click.bs.modal.data-api", '[data-toggle="modal"]', function (i) { var o = t(this), n = o.attr("href"), s = t(o.attr("data-target") || n && n.replace(/.*(?=#[^\s]+$)/, "")), a = s.data("bs.modal") ? "toggle" : t.extend({ remote: !/#/.test(n) && n }, s.data(), o.data()); o.is("a") && i.preventDefault(), s.one("show.bs.modal", function (t) { t.isDefaultPrevented() || s.one("hidden.bs.modal", function () { o.is(":visible") && o.trigger("focus") }) }), e.call(s, a, this) }) } (jQuery), +function (t) { "use strict"; function e(e) { return this.each(function () { var o = t(this), n = o.data("bs.tooltip"), s = "object" == typeof e && e; (n || !/destroy|hide/.test(e)) && (n || o.data("bs.tooltip", n = new i(this, s)), "string" == typeof e && n[e]()) }) } var i = function (t, e) { this.type = null, this.options = null, this.enabled = null, this.timeout = null, this.hoverState = null, this.$element = null, this.init("tooltip", t, e) }; i.VERSION = "3.3.4", i.TRANSITION_DURATION = 150, i.DEFAULTS = { animation: !0, placement: "top", selector: !1, template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>', trigger: "hover focus", title: "", delay: 0, html: !1, container: !1, viewport: { selector: "body", padding: 0} }, i.prototype.init = function (e, i, o) { if (this.enabled = !0, this.type = e, this.$element = t(i), this.options = this.getOptions(o), this.$viewport = this.options.viewport && t(this.options.viewport.selector || this.options.viewport), this.$element[0] instanceof document.constructor && !this.options.selector) throw new Error("`selector` option must be specified when initializing " + this.type + " on the window.document object!"); for (var n = this.options.trigger.split(" "), s = n.length; s--; ) { var a = n[s]; if ("click" == a) this.$element.on("click." + this.type, this.options.selector, t.proxy(this.toggle, this)); else if ("manual" != a) { var r = "hover" == a ? "mouseenter" : "focusin", l = "hover" == a ? "mouseleave" : "focusout"; this.$element.on(r + "." + this.type, this.options.selector, t.proxy(this.enter, this)), this.$element.on(l + "." + this.type, this.options.selector, t.proxy(this.leave, this)) } } this.options.selector ? this._options = t.extend({}, this.options, { trigger: "manual", selector: "" }) : this.fixTitle() }, i.prototype.getDefaults = function () { return i.DEFAULTS }, i.prototype.getOptions = function (e) { return e = t.extend({}, this.getDefaults(), this.$element.data(), e), e.delay && "number" == typeof e.delay && (e.delay = { show: e.delay, hide: e.delay }), e }, i.prototype.getDelegateOptions = function () { var e = {}, i = this.getDefaults(); return this._options && t.each(this._options, function (t, o) { i[t] != o && (e[t] = o) }), e }, i.prototype.enter = function (e) { var i = e instanceof this.constructor ? e : t(e.currentTarget).data("bs." + this.type); return i && i.$tip && i.$tip.is(":visible") ? void (i.hoverState = "in") : (i || (i = new this.constructor(e.currentTarget, this.getDelegateOptions()), t(e.currentTarget).data("bs." + this.type, i)), clearTimeout(i.timeout), i.hoverState = "in", i.options.delay && i.options.delay.show ? void (i.timeout = setTimeout(function () { "in" == i.hoverState && i.show() }, i.options.delay.show)) : i.show()) }, i.prototype.leave = function (e) { var i = e instanceof this.constructor ? e : t(e.currentTarget).data("bs." + this.type); return i || (i = new this.constructor(e.currentTarget, this.getDelegateOptions()), t(e.currentTarget).data("bs." + this.type, i)), clearTimeout(i.timeout), i.hoverState = "out", i.options.delay && i.options.delay.hide ? void (i.timeout = setTimeout(function () { "out" == i.hoverState && i.hide() }, i.options.delay.hide)) : i.hide() }, i.prototype.show = function () { var e = t.Event("show.bs." + this.type); if (this.hasContent() && this.enabled) { this.$element.trigger(e); var o = t.contains(this.$element[0].ownerDocument.documentElement, this.$element[0]); if (e.isDefaultPrevented() || !o) return; var n = this, s = this.tip(), a = this.getUID(this.type); this.setContent(), s.attr("id", a), this.$element.attr("aria-describedby", a), this.options.animation && s.addClass("fade"); var r = "function" == typeof this.options.placement ? this.options.placement.call(this, s[0], this.$element[0]) : this.options.placement, l = /\s?auto?\s?/i, h = l.test(r); h && (r = r.replace(l, "") || "top"), s.detach().css({ top: 0, left: 0, display: "block" }).addClass(r).data("bs." + this.type, this), this.options.container ? s.appendTo(this.options.container) : s.insertAfter(this.$element); var d = this.getPosition(), p = s[0].offsetWidth, c = s[0].offsetHeight; if (h) { var f = r, u = this.options.container ? t(this.options.container) : this.$element.parent(), g = this.getPosition(u); r = "bottom" == r && d.bottom + c > g.bottom ? "top" : "top" == r && d.top - c < g.top ? "bottom" : "right" == r && d.right + p > g.width ? "left" : "left" == r && d.left - p < g.left ? "right" : r, s.removeClass(f).addClass(r) } var m = this.getCalculatedOffset(r, d, p, c); this.applyPlacement(m, r); var v = function () { var t = n.hoverState; n.$element.trigger("shown.bs." + n.type), n.hoverState = null, "out" == t && n.leave(n) }; t.support.transition && this.$tip.hasClass("fade") ? s.one("bsTransitionEnd", v).emulateTransitionEnd(i.TRANSITION_DURATION) : v() } }, i.prototype.applyPlacement = function (e, i) { var o = this.tip(), n = o[0].offsetWidth, s = o[0].offsetHeight, a = parseInt(o.css("margin-top"), 10), r = parseInt(o.css("margin-left"), 10); isNaN(a) && (a = 0), isNaN(r) && (r = 0), e.top = e.top + a, e.left = e.left + r, t.offset.setOffset(o[0], t.extend({ using: function (t) { o.css({ top: Math.round(t.top), left: Math.round(t.left) }) } }, e), 0), o.addClass("in"); var l = o[0].offsetWidth, h = o[0].offsetHeight; "top" == i && h != s && (e.top = e.top + s - h); var d = this.getViewportAdjustedDelta(i, e, l, h); d.left ? e.left += d.left : e.top += d.top; var p = /top|bottom/.test(i), c = p ? 2 * d.left - n + l : 2 * d.top - s + h, f = p ? "offsetWidth" : "offsetHeight"; o.offset(e), this.replaceArrow(c, o[0][f], p) }, i.prototype.replaceArrow = function (t, e, i) { this.arrow().css(i ? "left" : "top", 50 * (1 - t / e) + "%").css(i ? "top" : "left", "") }, i.prototype.setContent = function () { var t = this.tip(), e = this.getTitle(); t.find(".tooltip-inner")[this.options.html ? "html" : "text"](e), t.removeClass("fade in top bottom left right") }, i.prototype.hide = function (e) { function o() { "in" != n.hoverState && s.detach(), n.$element.removeAttr("aria-describedby").trigger("hidden.bs." + n.type), e && e() } var n = this, s = t(this.$tip), a = t.Event("hide.bs." + this.type); return this.$element.trigger(a), a.isDefaultPrevented() ? void 0 : (s.removeClass("in"), t.support.transition && s.hasClass("fade") ? s.one("bsTransitionEnd", o).emulateTransitionEnd(i.TRANSITION_DURATION) : o(), this.hoverState = null, this) }, i.prototype.fixTitle = function () { var t = this.$element; (t.attr("title") || "string" != typeof t.attr("data-original-title")) && t.attr("data-original-title", t.attr("title") || "").attr("title", "") }, i.prototype.hasContent = function () { return this.getTitle() }, i.prototype.getPosition = function (e) { e = e || this.$element; var i = e[0], o = "BODY" == i.tagName, n = i.getBoundingClientRect(); null == n.width && (n = t.extend({}, n, { width: n.right - n.left, height: n.bottom - n.top })); var s = o ? { top: 0, left: 0} : e.offset(), a = { scroll: o ? document.documentElement.scrollTop || document.body.scrollTop : e.scrollTop() }, r = o ? { width: t(window).width(), height: t(window).height()} : null; return t.extend({}, n, a, r, s) }, i.prototype.getCalculatedOffset = function (t, e, i, o) { return "bottom" == t ? { top: e.top + e.height, left: e.left + e.width / 2 - i / 2} : "top" == t ? { top: e.top - o, left: e.left + e.width / 2 - i / 2} : "left" == t ? { top: e.top + e.height / 2 - o / 2, left: e.left - i} : { top: e.top + e.height / 2 - o / 2, left: e.left + e.width} }, i.prototype.getViewportAdjustedDelta = function (t, e, i, o) { var n = { top: 0, left: 0 }; if (!this.$viewport) return n; var s = this.options.viewport && this.options.viewport.padding || 0, a = this.getPosition(this.$viewport); if (/right|left/.test(t)) { var r = e.top - s - a.scroll, l = e.top + s - a.scroll + o; r < a.top ? n.top = a.top - r : l > a.top + a.height && (n.top = a.top + a.height - l) } else { var h = e.left - s, d = e.left + s + i; h < a.left ? n.left = a.left - h : d > a.width && (n.left = a.left + a.width - d) } return n }, i.prototype.getTitle = function () { var t, e = this.$element, i = this.options; return t = e.attr("data-original-title") || ("function" == typeof i.title ? i.title.call(e[0]) : i.title) }, i.prototype.getUID = function (t) { do t += ~ ~(1e6 * Math.random()); while (document.getElementById(t)); return t }, i.prototype.tip = function () { return this.$tip = this.$tip || t(this.options.template) }, i.prototype.arrow = function () { return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow") }, i.prototype.enable = function () { this.enabled = !0 }, i.prototype.disable = function () { this.enabled = !1 }, i.prototype.toggleEnabled = function () { this.enabled = !this.enabled }, i.prototype.toggle = function (e) { var i = this; e && (i = t(e.currentTarget).data("bs." + this.type), i || (i = new this.constructor(e.currentTarget, this.getDelegateOptions()), t(e.currentTarget).data("bs." + this.type, i))), i.tip().hasClass("in") ? i.leave(i) : i.enter(i) }, i.prototype.destroy = function () { var t = this; clearTimeout(this.timeout), this.hide(function () { t.$element.off("." + t.type).removeData("bs." + t.type) }) }; var o = t.fn.tooltip; t.fn.tooltip = e, t.fn.tooltip.Constructor = i, t.fn.tooltip.noConflict = function () { return t.fn.tooltip = o, this } } (jQuery), +function (t) { "use strict"; function e(e) { return this.each(function () { var o = t(this), n = o.data("bs.popover"), s = "object" == typeof e && e; (n || !/destroy|hide/.test(e)) && (n || o.data("bs.popover", n = new i(this, s)), "string" == typeof e && n[e]()) }) } var i = function (t, e) { this.init("popover", t, e) }; if (!t.fn.tooltip) throw new Error("Popover requires tooltip.js"); i.VERSION = "3.3.4", i.DEFAULTS = t.extend({}, t.fn.tooltip.Constructor.DEFAULTS, { placement: "right", trigger: "click", content: "", template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>' }), i.prototype = t.extend({}, t.fn.tooltip.Constructor.prototype), i.prototype.constructor = i, i.prototype.getDefaults = function () { return i.DEFAULTS }, i.prototype.setContent = function () { var t = this.tip(), e = this.getTitle(), i = this.getContent(); t.find(".popover-title")[this.options.html ? "html" : "text"](e), t.find(".popover-content").children().detach().end()[this.options.html ? "string" == typeof i ? "html" : "append" : "text"](i), t.removeClass("fade top bottom left right in"), t.find(".popover-title").html() || t.find(".popover-title").hide() }, i.prototype.hasContent = function () { return this.getTitle() || this.getContent() }, i.prototype.getContent = function () { var t = this.$element, e = this.options; return t.attr("data-content") || ("function" == typeof e.content ? e.content.call(t[0]) : e.content) }, i.prototype.arrow = function () { return this.$arrow = this.$arrow || this.tip().find(".arrow") }; var o = t.fn.popover; t.fn.popover = e, t.fn.popover.Constructor = i, t.fn.popover.noConflict = function () { return t.fn.popover = o, this } } (jQuery), +function (t) { "use strict"; function e(e) { return this.each(function () { var o = t(this), n = o.data("bs.tab"); n || o.data("bs.tab", n = new i(this)), "string" == typeof e && n[e]() }) } var i = function (e) { this.element = t(e) }; i.VERSION = "3.3.4", i.TRANSITION_DURATION = 150, i.prototype.show = function () { var e = this.element, i = e.closest("ul:not(.dropdown-menu)"), o = e.data("target"); if (o || (o = e.attr("href"), o = o && o.replace(/.*(?=#[^\s]*$)/, "")), !e.parent("li").hasClass("active")) { var n = i.find(".active:last a"), s = t.Event("hide.bs.tab", { relatedTarget: e[0] }), a = t.Event("show.bs.tab", { relatedTarget: n[0] }); if (n.trigger(s), e.trigger(a), !a.isDefaultPrevented() && !s.isDefaultPrevented()) { var r = t(o); this.activate(e.closest("li"), i), this.activate(r, r.parent(), function () { n.trigger({ type: "hidden.bs.tab", relatedTarget: e[0] }), e.trigger({ type: "shown.bs.tab", relatedTarget: n[0] }) }) } } }, i.prototype.activate = function (e, o, n) { function s() { a.removeClass("active").find("> .dropdown-menu > .active").removeClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !1), e.addClass("active").find('[data-toggle="tab"]').attr("aria-expanded", !0), r ? (e[0].offsetWidth, e.addClass("in")) : e.removeClass("fade"), e.parent(".dropdown-menu").length && e.closest("li.dropdown").addClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !0), n && n() } var a = o.find("> .active"), r = n && t.support.transition && (a.length && a.hasClass("fade") || !!o.find("> .fade").length); a.length && r ? a.one("bsTransitionEnd", s).emulateTransitionEnd(i.TRANSITION_DURATION) : s(), a.removeClass("in") }; var o = t.fn.tab; t.fn.tab = e, t.fn.tab.Constructor = i, t.fn.tab.noConflict = function () { return t.fn.tab = o, this }; var n = function (i) { i.preventDefault(), e.call(t(this), "show") }; t(document).on("click.bs.tab.data-api", '[data-toggle="tab"]', n).on("click.bs.tab.data-api", '[data-toggle="pill"]', n) } (jQuery), +function (t) { "use strict"; function e(e) { return this.each(function () { var o = t(this), n = o.data("bs.affix"), s = "object" == typeof e && e; n || o.data("bs.affix", n = new i(this, s)), "string" == typeof e && n[e]() }) } var i = function (e, o) { this.options = t.extend({}, i.DEFAULTS, o), this.$target = t(this.options.target).on("scroll.bs.affix.data-api", t.proxy(this.checkPosition, this)).on("click.bs.affix.data-api", t.proxy(this.checkPositionWithEventLoop, this)), this.$element = t(e), this.affixed = null, this.unpin = null, this.pinnedOffset = null, this.checkPosition() }; i.VERSION = "3.3.4", i.RESET = "affix affix-top affix-bottom", i.DEFAULTS = { offset: 0, target: window }, i.prototype.getState = function (t, e, i, o) { var n = this.$target.scrollTop(), s = this.$element.offset(), a = this.$target.height(); if (null != i && "top" == this.affixed) return i > n ? "top" : !1; if ("bottom" == this.affixed) return null != i ? n + this.unpin <= s.top ? !1 : "bottom" : t - o >= n + a ? !1 : "bottom"; var r = null == this.affixed, l = r ? n : s.top, h = r ? a : e; return null != i && i >= n ? "top" : null != o && l + h >= t - o ? "bottom" : !1 }, i.prototype.getPinnedOffset = function () { if (this.pinnedOffset) return this.pinnedOffset; this.$element.removeClass(i.RESET).addClass("affix"); var t = this.$target.scrollTop(), e = this.$element.offset(); return this.pinnedOffset = e.top - t }, i.prototype.checkPositionWithEventLoop = function () { setTimeout(t.proxy(this.checkPosition, this), 1) }, i.prototype.checkPosition = function () { if (this.$element.is(":visible")) { var e = this.$element.height(), o = this.options.offset, n = o.top, s = o.bottom, a = t(document.body).height(); "object" != typeof o && (s = n = o), "function" == typeof n && (n = o.top(this.$element)), "function" == typeof s && (s = o.bottom(this.$element)); var r = this.getState(a, e, n, s); if (this.affixed != r) { null != this.unpin && this.$element.css("top", ""); var l = "affix" + (r ? "-" + r : ""), h = t.Event(l + ".bs.affix"); if (this.$element.trigger(h), h.isDefaultPrevented()) return; this.affixed = r, this.unpin = "bottom" == r ? this.getPinnedOffset() : null, this.$element.removeClass(i.RESET).addClass(l).trigger(l.replace("affix", "affixed") + ".bs.affix") } "bottom" == r && this.$element.offset({ top: a - e - s }) } }; var o = t.fn.affix; t.fn.affix = e, t.fn.affix.Constructor = i, t.fn.affix.noConflict = function () { return t.fn.affix = o, this }, t(window).on("load", function () { t('[data-spy="affix"]').each(function () { var i = t(this), o = i.data(); o.offset = o.offset || {}, null != o.offsetBottom && (o.offset.bottom = o.offsetBottom), null != o.offsetTop && (o.offset.top = o.offsetTop), e.call(i, o) }) }) } (jQuery), +function (t) {
    "use strict"; function e(e) { var i, o = e.attr("data-target") || (i = e.attr("href")) && i.replace(/.*(?=#[^\s]+$)/, ""); return t(o) } function i(e) { return this.each(function () { var i = t(this), n = i.data("bs.collapse"), s = t.extend({}, o.DEFAULTS, i.data(), "object" == typeof e && e); !n && s.toggle && /show|hide/.test(e) && (s.toggle = !1), n || i.data("bs.collapse", n = new o(this, s)), "string" == typeof e && n[e]() }) } var o = function (e, i) { this.$element = t(e), this.options = t.extend({}, o.DEFAULTS, i), this.$trigger = t('[data-toggle="collapse"][href="#' + e.id + '"],[data-toggle="collapse"][data-target="#' + e.id + '"]'), this.transitioning = null, this.options.parent ? this.$parent = this.getParent() : this.addAriaAndCollapsedClass(this.$element, this.$trigger), this.options.toggle && this.toggle() }; o.VERSION = "3.3.4", o.TRANSITION_DURATION = 350, o.DEFAULTS = { toggle: !0 }, o.prototype.dimension = function () { var t = this.$element.hasClass("width"); return t ? "width" : "height" }, o.prototype.show = function () { if (!this.transitioning && !this.$element.hasClass("in")) { var e, n = this.$parent && this.$parent.children(".panel").children(".in, .collapsing"); if (!(n && n.length && (e = n.data("bs.collapse"), e && e.transitioning))) { var s = t.Event("show.bs.collapse"); if (this.$element.trigger(s), !s.isDefaultPrevented()) { n && n.length && (i.call(n, "hide"), e || n.data("bs.collapse", null)); var a = this.dimension(); this.$element.removeClass("collapse").addClass("collapsing")[a](0).attr("aria-expanded", !0), this.$trigger.removeClass("collapsed").attr("aria-expanded", !0), this.transitioning = 1; var r = function () { this.$element.removeClass("collapsing").addClass("collapse in")[a](""), this.transitioning = 0, this.$element.trigger("shown.bs.collapse") }; if (!t.support.transition) return r.call(this); var l = t.camelCase(["scroll", a].join("-")); this.$element.one("bsTransitionEnd", t.proxy(r, this)).emulateTransitionEnd(o.TRANSITION_DURATION)[a](this.$element[0][l]) } } } }, o.prototype.hide = function () { if (!this.transitioning && this.$element.hasClass("in")) { var e = t.Event("hide.bs.collapse"); if (this.$element.trigger(e), !e.isDefaultPrevented()) { var i = this.dimension(); this.$element[i](this.$element[i]())[0].offsetHeight, this.$element.addClass("collapsing").removeClass("collapse in").attr("aria-expanded", !1), this.$trigger.addClass("collapsed").attr("aria-expanded", !1), this.transitioning = 1; var n = function () { this.transitioning = 0, this.$element.removeClass("collapsing").addClass("collapse").trigger("hidden.bs.collapse") }; return t.support.transition ? void this.$element[i](0).one("bsTransitionEnd", t.proxy(n, this)).emulateTransitionEnd(o.TRANSITION_DURATION) : n.call(this) } } }, o.prototype.toggle = function () { this[this.$element.hasClass("in") ? "hide" : "show"]() }, o.prototype.getParent = function () {
        return t(this.options.parent).find('[data-toggle="collapse"][data-parent="' + this.options.parent + '"]').each(t.proxy(function (i, o) {
            var n = t(o);
            this.addAriaAndCollapsedClass(e(n), n)
        }, this)).end()
    }, o.prototype.addAriaAndCollapsedClass = function (t, e) { var i = t.hasClass("in"); t.attr("aria-expanded", i), e.toggleClass("collapsed", !i).attr("aria-expanded", i) }; var n = t.fn.collapse; t.fn.collapse = i, t.fn.collapse.Constructor = o, t.fn.collapse.noConflict = function () { return t.fn.collapse = n, this }, t(document).on("click.bs.collapse.data-api", '[data-toggle="collapse"]', function (o) { var n = t(this); n.attr("data-target") || o.preventDefault(); var s = e(n), a = s.data("bs.collapse"), r = a ? "toggle" : n.data(); i.call(s, r) })
} (jQuery), +function (t) { "use strict"; function e(i, o) { this.$body = t(document.body), this.$scrollElement = t(t(i).is(document.body) ? window : i), this.options = t.extend({}, e.DEFAULTS, o), this.selector = (this.options.target || "") + " .nav li > a", this.offsets = [], this.targets = [], this.activeTarget = null, this.scrollHeight = 0, this.$scrollElement.on("scroll.bs.scrollspy", t.proxy(this.process, this)), this.refresh(), this.process() } function i(i) { return this.each(function () { var o = t(this), n = o.data("bs.scrollspy"), s = "object" == typeof i && i; n || o.data("bs.scrollspy", n = new e(this, s)), "string" == typeof i && n[i]() }) } e.VERSION = "3.3.4", e.DEFAULTS = { offset: 10 }, e.prototype.getScrollHeight = function () { return this.$scrollElement[0].scrollHeight || Math.max(this.$body[0].scrollHeight, document.documentElement.scrollHeight) }, e.prototype.refresh = function () { var e = this, i = "offset", o = 0; this.offsets = [], this.targets = [], this.scrollHeight = this.getScrollHeight(), t.isWindow(this.$scrollElement[0]) || (i = "position", o = this.$scrollElement.scrollTop()), this.$body.find(this.selector).map(function () { var e = t(this), n = e.data("target") || e.attr("href"), s = /^#./.test(n) && t(n); return s && s.length && s.is(":visible") && [[s[i]().top + o, n]] || null }).sort(function (t, e) { return t[0] - e[0] }).each(function () { e.offsets.push(this[0]), e.targets.push(this[1]) }) }, e.prototype.process = function () { var t, e = this.$scrollElement.scrollTop() + this.options.offset, i = this.getScrollHeight(), o = this.options.offset + i - this.$scrollElement.height(), n = this.offsets, s = this.targets, a = this.activeTarget; if (this.scrollHeight != i && this.refresh(), e >= o) return a != (t = s[s.length - 1]) && this.activate(t); if (a && e < n[0]) return this.activeTarget = null, this.clear(); for (t = n.length; t--; ) a != s[t] && e >= n[t] && (void 0 === n[t + 1] || e < n[t + 1]) && this.activate(s[t]) }, e.prototype.activate = function (e) { this.activeTarget = e, this.clear(); var i = this.selector + '[data-target="' + e + '"],' + this.selector + '[href="' + e + '"]', o = t(i).parents("li").addClass("active"); o.parent(".dropdown-menu").length && (o = o.closest("li.dropdown").addClass("active")), o.trigger("activate.bs.scrollspy") }, e.prototype.clear = function () { t(this.selector).parentsUntil(this.options.target, ".active").removeClass("active") }; var o = t.fn.scrollspy; t.fn.scrollspy = i, t.fn.scrollspy.Constructor = e, t.fn.scrollspy.noConflict = function () { return t.fn.scrollspy = o, this }, t(window).on("load.bs.scrollspy.data-api", function () { t('[data-spy="scroll"]').each(function () { var e = t(this); i.call(e, e.data()) }) }) } (jQuery), +function (t) { "use strict"; function e() { var t = document.createElement("bootstrap"), e = { WebkitTransition: "webkitTransitionEnd", MozTransition: "transitionend", OTransition: "oTransitionEnd otransitionend", transition: "transitionend" }; for (var i in e) if (void 0 !== t.style[i]) return { end: e[i] }; return !1 } t.fn.emulateTransitionEnd = function (e) { var i = !1, o = this; t(this).one("bsTransitionEnd", function () { i = !0 }); var n = function () { i || t(o).trigger(t.support.transition.end) }; return setTimeout(n, e), this }, t(function () { t.support.transition = e(), t.support.transition && (t.event.special.bsTransitionEnd = { bindType: t.support.transition.end, delegateType: t.support.transition.end, handle: function (e) { return t(e.target).is(this) ? e.handleObj.handler.apply(this, arguments) : void 0 } }) }) } (jQuery);



/* include modernizr.js */

window.Modernizr = function (n, t, i) { function a(n) { y.cssText = n } function e(n, t) { return typeof n === t } function k(n, t) { return !! ~("" + n).indexOf(t) } function d(n, t) { var u, r; for (u in n) if (r = n[u], !k(r, "-") && y[r] !== i) return t == "pfx" ? r : !0; return !1 } function ft(n, t, r) { var f, u; for (f in n) if (u = t[n[f]], u !== i) return r === !1 ? n[f] : e(u, "function") ? u.bind(r || t) : u; return !1 } function g(n, t, i) { var r = n.charAt(0).toUpperCase() + n.slice(1), u = (n + " " + it.join(r + " ") + r).split(" "); return e(t, "string") || e(t, "undefined") ? d(u, t) : (u = (n + " " + rt.join(r + " ") + r).split(" "), ft(u, t, i)) } var et = "2.6.2", r = {}, v = !0, u = t.documentElement, o = "modernizr", nt = t.createElement(o), y = nt.style, ot, st = {}.toString, s = " -webkit- -moz- -o- -ms- ".split(" "), tt = "Webkit Moz O ms", it = tt.split(" "), rt = tt.toLowerCase().split(" "), f = {}, ht = {}, ct = {}, p = [], w = p.slice, h, ut = function (n, i, r, f) { var l, a, c, v, e = t.createElement("div"), h = t.body, s = h || t.createElement("body"); if (parseInt(r, 10)) while (r--) c = t.createElement("div"), c.id = f ? f[r] : o + (r + 1), e.appendChild(c); return l = ["&#173;", '<style id="s', o, '">', n, "<\/style>"].join(""), e.id = o, (h ? e : s).innerHTML += l, s.appendChild(e), h || (s.style.background = "", s.style.overflow = "hidden", v = u.style.overflow, u.style.overflow = "hidden", u.appendChild(s)), a = i(e, n), h ? e.parentNode.removeChild(e) : (s.parentNode.removeChild(s), u.style.overflow = v), !!a }, b = {}.hasOwnProperty, c, l; c = !e(b, "undefined") && !e(b.call, "undefined") ? function (n, t) { return b.call(n, t) } : function (n, t) { return t in n && e(n.constructor.prototype[t], "undefined") }, Function.prototype.bind || (Function.prototype.bind = function (n) { var t = this, i, r; if (typeof t != "function") throw new TypeError; return i = w.call(arguments, 1), r = function () { var f, e, u; return this instanceof r ? (f = function () { }, f.prototype = t.prototype, e = new f, u = t.apply(e, i.concat(w.call(arguments))), Object(u) === u ? u : e) : t.apply(n, i.concat(w.call(arguments))) }, r }), f.touch = function () { var i; return "ontouchstart" in n || n.DocumentTouch && t instanceof DocumentTouch ? i = !0 : ut(["@media (", s.join("touch-enabled),("), o, ")", "{#modernizr{top:9px;position:absolute}}"].join(""), function (n) { i = n.offsetTop === 9 }), i }, f.geolocation = function () { return "geolocation" in navigator }, f.cssgradients = function () { var n = "background-image:", t = "gradient(linear,left top,right bottom,from(#9f9),to(white));", i = "linear-gradient(left top,#9f9, white);"; return a((n + "-webkit- ".split(" ").join(t + n) + s.join(i + n)).slice(0, -n.length)), k(y.backgroundImage, "gradient") }, f.csstransitions = function () { return g("transition") }; for (l in f) c(f, l) && (h = l.toLowerCase(), r[h] = f[l](), p.push((r[h] ? "" : "no-") + h)); return r.addTest = function (n, t) { if (typeof n == "object") for (var f in n) c(n, f) && r.addTest(f, n[f]); else { if (n = n.toLowerCase(), r[n] !== i) return r; t = typeof t == "function" ? t() : t, typeof v != "undefined" && v && (u.className += " " + (t ? "" : "no-") + n), r[n] = t } return r }, a(""), nt = ot = null, function (n, t) { function v(n, t) { var i = n.createElement("p"), r = n.getElementsByTagName("head")[0] || n.documentElement; return i.innerHTML = "x<style>" + t + "<\/style>", r.insertBefore(i.lastChild, r.firstChild) } function s() { var n = r.elements; return typeof n == "string" ? n.split(" ") : n } function u(n) { var t = a[n[l]]; return t || (t = {}, o++, n[l] = o, a[o] = t), t } function h(n, r, f) { if (r || (r = t), i) return r.createElement(n); f || (f = u(r)); var e; return e = f.cache[n] ? f.cache[n].cloneNode() : b.test(n) ? (f.cache[n] = f.createElem(n)).cloneNode() : f.createElem(n), e.canHaveChildren && !w.test(n) ? f.frag.appendChild(e) : e } function y(n, r) { if (n || (n = t), i) return n.createDocumentFragment(); r = r || u(n); for (var e = r.frag.cloneNode(), f = 0, o = s(), h = o.length; f < h; f++) e.createElement(o[f]); return e } function p(n, t) { t.cache || (t.cache = {}, t.createElem = n.createElement, t.createFrag = n.createDocumentFragment, t.frag = t.createFrag()), n.createElement = function (i) { return r.shivMethods ? h(i, n, t) : t.createElem(i) }, n.createDocumentFragment = Function("h,f", "return function(){var n=f.cloneNode(),c=n.createElement;h.shivMethods&&(" + s().join().replace(/\w+/g, function (n) { return t.createElem(n), t.frag.createElement(n), 'c("' + n + '")' }) + ");return n}")(r, t.frag) } function c(n) { n || (n = t); var f = u(n); return r.shivCSS && !e && !f.hasCSS && (f.hasCSS = !!v(n, "article,aside,figcaption,figure,footer,header,hgroup,nav,section{display:block}mark{background:#FF0;color:#000}")), i || p(n, f), n } var f = n.html5 || {}, w = /^<|^(?:button|map|select|textarea|object|iframe|option|optgroup)$/i, b = /^(?:a|b|code|div|fieldset|h1|h2|h3|h4|h5|h6|i|label|li|ol|p|q|span|strong|style|table|tbody|td|th|tr|ul)$/i, e, l = "_html5shiv", o = 0, a = {}, i, r; (function () { try { var n = t.createElement("a"); n.innerHTML = "<xyz><\/xyz>", e = "hidden" in n, i = n.childNodes.length == 1 || function () { t.createElement("a"); var n = t.createDocumentFragment(); return typeof n.cloneNode == "undefined" || typeof n.createDocumentFragment == "undefined" || typeof n.createElement == "undefined" } () } catch (r) { e = !0, i = !0 } })(), r = { elements: f.elements || "abbr article aside audio bdi canvas data datalist details figcaption figure footer header hgroup mark meter nav output progress section summary time video", shivCSS: f.shivCSS !== !1, supportsUnknownElements: i, shivMethods: f.shivMethods !== !1, type: "default", shivDocument: c, createElement: h, createDocumentFragment: y }, n.html5 = r, c(t) } (this, t), r._version = et, r._prefixes = s, r._domPrefixes = rt, r._cssomPrefixes = it, r.testProp = function (n) { return d([n]) }, r.testAllProps = g, r.testStyles = ut, u.className = u.className.replace(/(^|\s)no-js(\s|$)/, "$1$2") + (v ? " js " + p.join(" ") : ""), r } (this, this.document)


/* Include allmasterscript.js */




function urlredirect() {
    try {
        var ddlattrvalue = document.getElementById('srcfield').value.replace("\"", "`~");
        //        .replace("%20", "_").replace(" ", "_").replace(/ /, '_').replace("/", "./.").replace("__", "_").replace("  ", "_");
        // alert(ddlattrvalue);
        if (ddlattrvalue == null || ddlattrvalue == "") {
            return false;
        }
        if (ddlattrvalue != "") {
            if (ddlattrvalue != "Quick Product search" || ddlattrvalue != "Quick Product search! Enter key words or product Codes!") {
                if (ttrim(ddlattrvalue) != "") {





                    $.ajax({
                        type: "POST",
                        url: "/GblWebMethods.aspx/stringreplace",
                        data: '{"strvalue":"' + ddlattrvalue + '"}',
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != "") {

                                window.location.href = "/" + data.d + "/ps/";

                            }
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            alert(err.Message);
                        }



                    });




                }
            }
        }
    }
    catch (e) {
        alert(e.ToString());
    }
}

function urlredirect_enterK(e) {

    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)


    var ddlattrvalue = document.getElementById("txtsear").value.replace("\"", "`~");

    if (ddlattrvalue != "") {
        if (ddlattrvalue != "Search wagner! Enter Keywords or Part No's") {

            if (e.keyCode == 13) {
                // window.document.location="ps.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");

                //  window.location.href = "ps.aspx?" + ddlattrvalue.replace(/#/, "%23").replace(/ /g, "_").replace("+", "%2B").replace(/\"/g, "%22").replace("%20", "_").replace("  ", "_").replace("__", "_"); 
                $.ajax({
                    type: "POST",
                    url: "/GblWebMethods.aspx/stringreplace",
                    data: '{"strvalue":"' + ddlattrvalue + '"}',
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d != "") {
                            window.location.href = "/" + data.d + "/ps/";
                        }
                    },
                    error: function (xhr, status, error) {
                        var err = eval("(" + xhr.responseText + ")");
                        alert(err.Message);
                    }



                });


                return false;
            }
        }
    }
}
function urlredirect_enter(e) {

    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)


    var ddlattrvalue = document.getElementById("txtsear1").value.replace("\"", "`~");

    if (ddlattrvalue != "") {
        if (ddlattrvalue != "Search wagner! Enter Keywords or Part No's") {

            if (e.keyCode == 13) {
                // window.document.location="ps.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");

                //  window.location.href = "ps.aspx?" + ddlattrvalue.replace(/#/, "%23").replace(/ /g, "_").replace("+", "%2B").replace(/\"/g, "%22").replace("%20", "_").replace("  ", "_").replace("__", "_"); 
                $.ajax({
                    type: "POST",
                    url: "/GblWebMethods.aspx/stringreplace",
                    data: '{"strvalue":"' + ddlattrvalue + '"}',
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d != "") {
                            window.location.href = "/" + data.d + "/ps/";
                        }
                    },
                    error: function (xhr, status, error) {
                        var err = eval("(" + xhr.responseText + ")");
                        alert(err.Message);
                    }



                });


                return false;
            }
        }
    }
}
function urlredirectK(e) {

    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)


    var ddlattrvalue = document.getElementById('txtsear').value.replace("\"", "`~");
    if (ddlattrvalue == null || ddlattrvalue == "") {
        return false;
    }
    if (ddlattrvalue != "") {
        if (ddlattrvalue != "Quick Product search" || ddlattrvalue != "Quick Product search! Enter key words or product Codes!") {

            // if (e.keyCode == 13) {
            // window.document.location="ps.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");

            //  window.location.href = "ps.aspx?" + ddlattrvalue.replace(/#/, "%23").replace(/ /g, "_").replace("+", "%2B").replace(/\"/g, "%22").replace("%20", "_").replace("  ", "_").replace("__", "_"); 
            $.ajax({
                type: "POST",
                url: "/GblWebMethods.aspx/stringreplace",
                data: '{"strvalue":"' + ddlattrvalue + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        window.location.href = "/" + data.d + "/ps/";
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }



            });


            return false;
            // }
        }
    }
}



function urlredirectK_automenu(e) {
    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    // var ddlattrvalue = document.getElementById('srcfield').value.replace("\"", "`~");

    var ddlattrvalue = document.getElementById('txtsearch_automenu').value.replace("\"", "`~");

    if (ddlattrvalue == null || ddlattrvalue == "") {
        return false;
    }
    if (ddlattrvalue != "") {
        if (ddlattrvalue != "Quick Product search") {

            // if (e.keyCode == 13) {
            // window.document.location="ps.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");

            //  window.location.href = "ps.aspx?" + ddlattrvalue.replace(/#/, "%23").replace(/ /g, "_").replace("+", "%2B").replace(/\"/g, "%22").replace("%20", "_").replace("  ", "_").replace("__", "_"); 
            $.ajax({
                type: "POST",
                url: "/GblWebMethods.aspx/stringreplace",
                data: '{"strvalue":"' + ddlattrvalue + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        window.location.href = "/" + data.d + "/ps/";
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }



            });


            return false;
            // }
        }
    }
}

function urlredirectK_src(e) {
    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    // var ddlattrvalue = document.getElementById('srcfield').value.replace("\"", "`~");

    var ddlattrvalue = document.getElementById('txtsear1').value.replace("\"", "`~");
    if (ddlattrvalue == null || ddlattrvalue == "") {
        return false;
    }
    if (ddlattrvalue != "") {
        if (ddlattrvalue != "Quick Product search" || ddlattrvalue != "Quick Product search! Enter key words or product Codes!") {

            // if (e.keyCode == 13) {
            // window.document.location="ps.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");

            //  window.location.href = "ps.aspx?" + ddlattrvalue.replace(/#/, "%23").replace(/ /g, "_").replace("+", "%2B").replace(/\"/g, "%22").replace("%20", "_").replace("  ", "_").replace("__", "_"); 
            $.ajax({
                type: "POST",
                url: "/GblWebMethods.aspx/stringreplace",
                data: '{"strvalue":"' + ddlattrvalue + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        window.location.href = "/" + data.d + "/ps/";
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }



            });


            return false;
            // }
        }
    }
}



function FillVal(ctl) {

    if (ctl.value == '' || ctl.value == null || ttrim(ctl.value) == '') {
        ctl.value = "Search wagner! Enter Keywords or Part No's";

        var id = document.getElementById("PSearchDiv");
        id.style.display = "none";
    }
}
function Foc(ctl) {
    if (ctl.value == "Search wagner! Enter Keywords or Part No's") {
        ctl.value = '';
    }
    if (ctl.value != '') {

        var id = document.getElementById("PSearchDiv");
        id.style.display = "block";
    }
}
var hovered = false;
function divMouseover(ctl) {
    hovered = true;
}
function divmouseout(ctl) {
    hovered = false;
}
function Focout(ctl) {
    if (!hovered) {
        var id = document.getElementById("PSearchDiv");
        id.style.display = "none";
    }


}
function Focout1(ctl) {

    var id = document.getElementById("PSearchDiv");
    id.style.display = "none";



}




function CheckTextPassMaxLength(textBox, e, length) {

    var mLen = textBox["MaxLength"];

    var tsellen = 0;
    var text = document.getElementById(textBox.id);
    var t = text.value.substr(text.selectionStart, text.selectionEnd - text.selectionStart);




    if (t != null) {
        tsellen = t.length;
    }
    if (null == mLen)
        mLen = length;

    var maxLength = parseInt(mLen);
    if (!checkSpecialKeys(e)) {
        if (textBox.value.length - tsellen > maxLength - 1) {
            if (window.event)//IE
            {
                alert("Password Length should not be greater than 15");
                e.returnValue = false;
            }
            else//Firefox
            {
                alert("Password Length should not be greater than 15");
                e.preventDefault();
            }
        }
    }
}

function checkUserName(e) {
    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    // if (keychar == "@" || keychar == "!" ||  keychar == "#" || keychar == "$" || keychar == "%" || keychar == "*" || keychar == "&" || keychar == "^" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" ){
    if (keychar == "~" || keychar == "`" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "^" || keychar == "&" || keychar == "*" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == "," || keychar == "<" || keychar == ">" || keychar == "?" || keychar == ";" || keychar == ":" || keychar == "{" || keychar == "}" || keychar == "[" || keychar == "]" || keychar == "|" || keychar == "'" || keychar == "/") {
        return false;
    }
    else {
        return true;
    }
}

function check(e) {
    var keynum
    var keychar
    var numcheck
    // For Internet Explorer
    if (window.event) {
        keynum = e.keyCode
    }
    // For Netscape/Firefox/Opera
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    //List of special characters you want to restrict
    if (keychar == "@" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "*" || keychar == "&" || keychar == "^" || keychar == "(" || keychar == ")" || keychar == "+") {
        e.keyCode = '';
        return false;
    }
    else {
        return true;
    }
}
function checkSpecialKeys(e) {
    if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
        return false;
    else
        return true;
}

function Email(e) {
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) {

        keynum = e.keyCode;
    }

    else if (e.which) {
        keynum = e.which;
    }

    keychar = String.fromCharCode(keynum);
    if (keychar == "~" || keychar == "`" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "^" || keychar == "&" || keychar == "*" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == "," || keychar == "<" || keychar == ">" || keychar == "?" || keychar == ";" || keychar == ":" || keychar == "{" || keychar == "}" || keychar == "[" || keychar == "]" || keychar == "|" || keychar == "'" || keychar == "/") {
        e.keyCode = '';
        return false;
    }
    else {
        return true;

    }
}


function blockspecialcharacters(e) {
    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    if (keychar == "@" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "*" || keychar == "&" || keychar == "^" || keychar == "(" || keychar == ")" || keychar == "+") {
        e.keyCode = '';
        return false;
    }
    else {
        return true;
    }
}






function urlredirectKM(e) {
    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)

    var ddlattrvalue = document.getElementById('srcfield').value.replace("\"", "`~");

    if (ddlattrvalue != "") {
        if (ddlattrvalue != "Quick Product search!") {


            $.ajax({
                type: "POST",
                url: "/GblWebMethods.aspx/stringreplace",
                data: '{"strvalue":"' + ddlattrvalue + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        window.location.href = "/" + data.d + "/ps/";
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }



            });


            return false;
            // }
        }
    }
}

function GetDeal() {

    var m = document.getElementById('txtMail1').value;

    window.location.href = '/GetDeal.aspx?mail=' + m;
}



/* include thickboxAddtocart.js */
/*
* Thickbox 3.1 - One Box To Rule Them All.
* By Cody Lindley (http://www.codylindley.com)
* Copyright (c) 2007 cody lindley
* Licensed under the MIT License: http://www.opensource.org/licenses/mit-license.php
 
*
*	Modified by JIN Weijie
*	Added previous/next by clicking the current image. Please visit http://www.jinweijie.com for detail
*/

var thickboxL10n = {
    next: "Next &gt;",
    prev: "&lt; Prev",
    image: "Image",
    of: "of",
    close: "Close",
    noiframes: "This feature requires inline frames. You have iframes disabled or your browser does not support them."
};

if (typeof tb_pathToImage != 'string') {
    var tb_pathToImage = "../../../../../../../../../../images/loadingAnimation.gif";
}
if (typeof tb_closeImage != 'string') {
    var tb_closeImage = "../../../../../../../../../../images/closebtn.png";
}

if (typeof tb_leftArrow != 'string') {
    var tb_leftArrow = "../../../../../../../../../../images/left.gif";
}

if (typeof tb_rightArrow != 'string') {
    var tb_rightArrow = "../../../../../../../../../../images/right.gif";
}


//on page load call tb_init
jQuery(document).ready(function () {
    tb_init('a.thickbox, area.thickbox, input.thickbox'); //pass where to apply thickbox
    imgLoader = new Image(); // preload image
    imgLoader.src = tb_pathToImage;
});

//add thickbox to href & area elements that have a class of .thickbox
function tb_init(domChunk) {
    // jQuery(domChunk).live('click', tb_click);
}

function tb_click() {
    var t = this.title || this.name || null;
    var a = this.href || this.alt;
    var g = this.rel || false;
    tb_show(t, a, g);
    this.blur();
    return false;
}

function tb_show(caption, url, imageGroup) {//function called when the user clicks on a thickbox link

    try {
        if (typeof document.body.style.maxHeight === "undefined") {//if IE 6
            jQuery("body", "html").css({ height: "100%", width: "100%" });
            jQuery("html").css("overflow", "hidden");
            if (document.getElementById("TB_HideSelect") === null) {//iframe to hide select elements in ie6
                jQuery("body").append("<iframe id='TB_HideSelect'>" + thickboxL10n.noiframes + "</iframe><div id='TB_overlay' ></div><div id='TB_window'></div>");
                jQuery("#TB_overlay").click(tb_remove);
            }
        } else {//all others
            if (document.getElementById("TB_overlay") === null) {
                var ovstyle = "display: block; background: none repeat scroll 0px 0px rgba(0, 0, 0, 0.6);";

                jQuery("body").append("<div id='TB_overlay' style='" + ovstyle + "'><div id='TB_window' ></div></div>");
                jQuery("#TB_overlay").click(tb_remove);
            }
        }

        if (tb_detectMacXFF()) {
            jQuery("#TB_overlay").addClass("TB_overlayMacFFBGHack"); //use png overlay so hide flash
        } else {
            jQuery("#TB_overlay").addClass("modal fade bs-example-modal-lg in"); //use background and opacity 
            //  jQuery("#TB_window").addClass("modal-dialog modal-lg");  
            jQuery("#TB_window").addClass("modal-dialog");
        }

        if (caption === null) { caption = ""; }
        jQuery("body").append("<div id='TB_load'> <span class='TB_load_content'> Please Wait...<br/><br/>Adding Item Cart </span><br/><br/><br/><br/><img src='" + imgLoader.src + "' /></div>"); //add loader to the page
        jQuery('#TB_load').show(); //show loader

        //jQuery("#TB_window").append("<img src='" + tb_closeImage + "' style='position:absolute;right:-20px;top:-22px;cursor: pointer; cursor: hand;' onClick='tb_remove()' />"); //add loader to the page

        // return;

        var baseURL;
        if (url.indexOf("?") !== -1) { //ff there is a query string involved
            baseURL = url.substr(0, url.indexOf("?"));
        } else {
            baseURL = url;
        }

        var urlString = /\.jpg$|\.jpeg$|\.png$|\.gif$|\.bmp$/;
        var urlType = baseURL.toLowerCase().match(urlString);

        if (urlType == '.jpg' || urlType == '.jpeg' || urlType == '.png' || urlType == '.gif' || urlType == '.bmp') {//code to show images

            TB_PrevCaption = "";
            TB_PrevURL = "";
            TB_PrevHTML = "";
            TB_NextCaption = "";
            TB_NextURL = "";
            TB_NextHTML = "";
            TB_imageCount = "";
            TB_FoundURL = false;
            if (imageGroup) {
                TB_TempArray = jQuery("a[rel=" + imageGroup + "]").get();
                for (TB_Counter = 0; ((TB_Counter < TB_TempArray.length) && (TB_NextHTML === "")); TB_Counter++) {
                    var urlTypeTemp = TB_TempArray[TB_Counter].href.toLowerCase().match(urlString);
                    if (!(TB_TempArray[TB_Counter].href == url)) {
                        if (TB_FoundURL) {
                            TB_NextCaption = TB_TempArray[TB_Counter].title;
                            TB_NextURL = TB_TempArray[TB_Counter].href;
                            TB_NextHTML = "<span id='TB_next'>&nbsp;&nbsp;<a href='#'>" + thickboxL10n.next + "</a></span>";
                        } else {
                            TB_PrevCaption = TB_TempArray[TB_Counter].title;
                            TB_PrevURL = TB_TempArray[TB_Counter].href;
                            TB_PrevHTML = "<span id='TB_prev'>&nbsp;&nbsp;<a href='#'>" + thickboxL10n.prev + "</a></span>";
                        }
                    } else {
                        TB_FoundURL = true;
                        TB_imageCount = thickboxL10n.image + ' ' + (TB_Counter + 1) + ' ' + thickboxL10n.of + ' ' + (TB_TempArray.length);
                    }
                }
            }


            imgPreloader = new Image();
            prevImg = new Image();
            nextImg = new Image();
            imgPreloader.onload = function () {
                imgPreloader.onload = null;

                var tb_links = jQuery('a[class="thickbox"]');
                var i = -1;
                tb_links.each(function (n) { if (this.href == imgPreloader.src) { i = n; } });


                if (i != -1) {
                    if (i > 0) { prevImg.src = tb_links[i - 1].href; }
                    if (i + 1 < tb_links.length) {

                        var imgTemp1 = new Image();
                        imgTemp1.src = tb_links[i + 1].href;

                        if (tb_links[i + 2]) {
                            var imgTemp2 = new Image();
                            imgTemp2.src = tb_links[i + 2].href;
                        }

                        if (tb_links[i + 3]) {
                            var imgTemp3 = new Image();
                            imgTemp3.src = tb_links[i + 3].href;
                        }
                    }
                }


                // imgPreloader = new Image();
                // imgPreloader.onload = function(){
                //  imgPreloader.onload = null;

                // Resizing large images - orginal by Christian Montoya edited by me.
                var pagesize = tb_getPageSize();
                var x = pagesize[0] - 150;
                var y = pagesize[1] - 150;
                var imageWidth = imgPreloader.width;
                var imageHeight = imgPreloader.height;
                if (imageWidth > x) {
                    imageHeight = imageHeight * (x / imageWidth);
                    imageWidth = x;
                    if (imageHeight > y) {
                        imageWidth = imageWidth * (y / imageHeight);
                        imageHeight = y;
                    }
                } else if (imageHeight > y) {
                    imageWidth = imageWidth * (y / imageHeight);
                    imageHeight = y;
                    if (imageWidth > x) {
                        imageHeight = imageHeight * (x / imageWidth);
                        imageWidth = x;
                    }
                }
                // End Resizing

                TB_WIDTH = imageWidth + 30;
                TB_HEIGHT = imageHeight + 60;
                jQuery("#TB_window").append("<img id='imgLeftArrow' src='" + tb_leftArrow + "' border='0' style='display:none;'/><img id='imgRightArrow' src='" + tb_rightArrow + "' border='0' style='display:none;'/><div id='divNavControl'><div id='divPre'></div><div id='divNext'></div></div><a href='#' id='TB_nextPIC' title='" + thickboxL10n.next + "'><img id='TB_Image' src='" + url + "' width='" + imageWidth + "' height='" + imageHeight + "' alt='" + caption + "'/></a>" + "<div id='TB_caption'>" + caption + "<div id='TB_secondLine'>" + TB_imageCount + TB_PrevHTML + TB_NextHTML + "</div></div><div id='TB_closeWindow'><a href='#' id='TB_closeWindowButton' title='" + thickboxL10n.close + "'><img src='" + tb_closeImage + "' /></a></div>");

                jQuery("#TB_closeWindowButton").click(tb_remove);

                function hideArrows() {
                    jQuery("#imgLeftArrow").css({ display: 'none' });
                    jQuery("#imgRightArrow").css({ display: 'none' });
                }

                jQuery("#divNavControl").css({ height: imageHeight + 'px', width: imageWidth + 'px', left: '15px', top: '15px' });

                if (!(TB_PrevHTML === "")) {
                    function goPrev() {
                        if (jQuery(document).unbind("click", goPrev)) { jQuery(document).unbind("click", goPrev); }
                        jQuery("#TB_window").remove();
                        jQuery("body").append("<div id='TB_window'></div>");
                        tb_show(TB_PrevCaption, TB_PrevURL, imageGroup);
                        return false;
                    }
                    jQuery("#TB_prev").click(goPrev);

                    function showLeftArrow() {
                        jQuery("#imgLeftArrow").css({ display: '' });
                        jQuery("#imgRightArrow").css({ display: 'none' });
                    }


                    jQuery("#divPre").click(goPrev);
                    jQuery("#divPre").mouseover(showLeftArrow);
                    jQuery("#divPre").mouseout(hideArrows);
                }

                if (!(TB_NextHTML === "")) {
                    function goNext() {
                        jQuery("#TB_window").remove();
                        jQuery("body").append("<div id='TB_window'></div>");
                        tb_show(TB_NextCaption, TB_NextURL, imageGroup);
                        return false;
                    }
                    jQuery("#TB_next").click(goNext);
                    jQuery("#TB_nextPIC").click(goNext);


                    jQuery("#divNext").click(goNext);

                    function showRightArrow() {
                        jQuery("#imgLeftArrow").css({ display: 'none' });
                        jQuery("#imgRightArrow").css({ display: '' });
                    }
                    jQuery("#divNext").mouseover(showRightArrow);
                    jQuery("#divNext").mouseout(hideArrows);

                }


                document.onkeydown = function (e) {
                    if (e == null) { // ie
                        keycode = event.keyCode;
                    } else { // mozilla
                        keycode = e.which;
                    }
                    if (keycode == 27) { // close
                        tb_remove();
                    } else if (keycode == 190) { // display previous image
                        if (!(TB_NextHTML == "")) {
                            document.onkeydown = "";
                            goNext();
                        }
                    } else if (keycode == 188) { // display next image
                        if (!(TB_PrevHTML == "")) {
                            document.onkeydown = "";
                            goPrev();
                        }
                    }
                };

                tb_position();
                jQuery("#TB_load").remove();
                jQuery("#TB_ImageOff").click(tb_remove);
                jQuery("#TB_window").css({ display: "block" }); //for safari using css instead of show
            };

            imgPreloader.src = url;
        } else {//code to show html

            var queryString = url.replace(/^[^\?]+\??/, '');
            var params = tb_parseQuery(queryString);

            TB_WIDTH = (params['width'] * 1) + 30 || 630; //defaults to 630 if no paramaters were added to URL
            TB_HEIGHT = (params['height'] * 1) + 40 || 440; //defaults to 440 if no paramaters were added to URL
            ajaxContentW = TB_WIDTH - 30;
            ajaxContentH = TB_HEIGHT - 45;

            if (url.indexOf('TB_iframe') != -1) {// either iframe or ajax window
                urlNoQuery = url.split('TB_');
                jQuery("#TB_iframeContent").remove();
                if (params['modal'] != "true") {//iframe no modal
                    jQuery("#TB_window").append("<div id='TB_title'><div id='TB_ajaxWindowTitle'>" + caption + "</div><div id='TB_closeAjaxWindow'><a href='#' id='TB_closeWindowButton' title='" + thickboxL10n.close + "'><img src='" + tb_closeImage + "' /></a></div></div><iframe frameborder='0' hspace='0' src='" + urlNoQuery[0] + "' id='TB_iframeContent' name='TB_iframeContent" + Math.round(Math.random() * 1000) + "' onload='tb_showIframe()' style='width:" + (ajaxContentW + 29) + "px;height:" + (ajaxContentH + 17) + "px;' >" + thickboxL10n.noiframes + "</iframe>");
                } else {//iframe modal
                    jQuery("#TB_overlay").unbind();
                    jQuery("#TB_window").append("<iframe frameborder='0' hspace='0' src='" + urlNoQuery[0] + "' id='TB_iframeContent' name='TB_iframeContent" + Math.round(Math.random() * 1000) + "' onload='tb_showIframe()' style='width:" + (ajaxContentW + 29) + "px;height:" + (ajaxContentH + 17) + "px;'>" + thickboxL10n.noiframes + "</iframe>");
                }
            } else {// not an iframe, ajax
                if (jQuery("#TB_window").css("display") != "block") {
                    if (params['modal'] != "true") {//ajax no modal
                        jQuery("#TB_window").append("<div id='TB_title'><div id='TB_ajaxWindowTitle'>" + caption + "</div><div id='TB_closeAjaxWindow'><a href='#' id='TB_closeWindowButton'><img src='" + tb_closeImage + "' /></a></div></div><div id='TB_ajaxContent' style='width:" + ajaxContentW + "px;height:" + ajaxContentH + "px'></div>");
                    } else {//ajax modal
                        jQuery("#TB_overlay").unbind();
                        jQuery("#TB_window").append("<div id='TB_ajaxContent' class='modal-content' ></div>");
                    }
                } else {//this means the window is already up, we are just loading new content via ajax
                    // jQuery("#TB_ajaxContent")[0].style.width = ajaxContentW + "px";
                    //jQuery("#TB_ajaxContent")[0].style.height = ajaxContentH + "px";
                    //jQuery("#TB_ajaxContent")[0].scrollTop = 0;
                    jQuery("#TB_ajaxWindowTitle").html(caption);
                }
            }

            jQuery("#TB_closeWindowButton").click(tb_remove);

            if (url.indexOf('TB_inline') != -1) {
                jQuery("#TB_ajaxContent").append(jQuery('#' + params['inlineId']).children());
                jQuery("#TB_window").unload(function () {
                    jQuery('#' + params['inlineId']).append(jQuery("#TB_ajaxContent").children()); // move elements back when you're finished
                });
                tb_position();
                jQuery("#TB_load").remove();
                jQuery("#TB_window").css({ display: "block" });
            } else if (url.indexOf('TB_iframe') != -1) {
                tb_position();
                if (jQuery.browser.safari) {//safari needs help because it will not fire iframe onload
                    jQuery("#TB_load").remove();
                    jQuery("#TB_window").css({ display: "block" });
                }
            } else {
                jQuery("#TB_ajaxContent").load(url += "&random=" + (new Date().getTime()), function () {//to do a post change this load method
                    tb_position();
                    jQuery("#TB_load").remove();
                    tb_init("#TB_ajaxContent a.thickbox");
                    jQuery("#TB_window").css({ display: "block" });
                });
            }

        }

        if (!params['modal']) {
            document.onkeyup = function (e) {
                if (e == null) { // ie
                    keycode = event.keyCode;
                } else { // mozilla
                    keycode = e.which;
                }
                if (keycode == 27) { // close
                    tb_remove();
                }
            };
        }

    } catch (e) {
        //nothing here
    }
}

//helper functions below
function tb_showIframe() {
    jQuery("#TB_load").remove();
    jQuery("#TB_window").css({ display: "block" });

}

function tb_remove() {
    jQuery("body").removeClass("modal-open");
    jQuery("#TB_imageOff").unbind("click");
    jQuery("#TB_closeWindowButton").unbind("click");
    jQuery("#TB_window").fadeOut("fast", function () { jQuery('#TB_window,#TB_overlay,#TB_HideSelect').trigger("unload").unbind().remove(); });
    jQuery("#TB_load").remove();
    if (typeof document.body.style.maxHeight == "undefined") {//if IE 6
        jQuery("body", "html").css({ height: "auto", width: "auto" });
        jQuery("html").css("overflow", "");
    }
    document.onkeydown = "";
    document.onkeyup = "";
    return false;
}

function tb_position() {
    var isIE6 = typeof document.body.style.maxHeight === "undefined";
    //  jQuery("#TB_window").css({ marginLeft: '-' + parseInt((TB_WIDTH / 2), 10) + 'px', width: TB_WIDTH + 'px' });
    if (!isIE6) { // take away IE6
        // jQuery("#TB_window").css({ marginTop: '-' + parseInt((TB_HEIGHT / 2), 10) + 'px' });
    }
}

function tb_parseQuery(query) {
    var Params = {};
    if (!query) { return Params; } // return empty object
    var Pairs = query.split(/[;&]/);
    for (var i = 0; i < Pairs.length; i++) {
        var KeyVal = Pairs[i].split('=');
        if (!KeyVal || KeyVal.length != 2) { continue; }
        var key = unescape(KeyVal[0]);
        var val = unescape(KeyVal[1]);
        val = val.replace(/\+/g, ' ');
        Params[key] = val;
    }
    return Params;
}

function tb_getPageSize() {
    var de = document.documentElement;
    var w = window.innerWidth || self.innerWidth || (de && de.clientWidth) || document.body.clientWidth;
    var h = window.innerHeight || self.innerHeight || (de && de.clientHeight) || document.body.clientHeight;
    arrayPageSize = [w, h];
    return arrayPageSize;
}

function tb_detectMacXFF() {
    var userAgent = navigator.userAgent.toLowerCase();
    if (userAgent.indexOf('mac') != -1 && userAgent.indexOf('firefox') != -1) {
        return true;
    }
}


function CallProductPopup(orgurl, buyvalue, pid, qtyval, tOrderID, fid) {

    var url = "";
    if (orgurl.toLowerCase().indexOf("/mpl") != -1 || orgurl.toLowerCase().indexOf("/mpd") != -1 || orgurl.toLowerCase().indexOf("/mct") != -1 || orgurl.toLowerCase().indexOf("/mps") != -1 || orgurl.toLowerCase().indexOf("/mfl") != -1) {

        if (tOrderID != null && parseInt(tOrderID) > 0) {
            url = orgurl + "mOrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval.trim() + "&ORDER_ID=" + tOrderID + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

        }
        else {
            url = orgurl + "mOrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval.trim() + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

        }
    }
    else {

        if (tOrderID != null && parseInt(tOrderID) > 0) {
            url = orgurl + "OrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval + "&ORDER_ID=" + tOrderID + "&popup=true&modal=true&width=350&height=380&fid=" + fid;
            // alert(url);
        }
        else {
            url = orgurl + "OrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

        }
    }

    tb_show(null, url, null);
    this.$("body").addClass("modal-open");

    // document.forms[0].elements[buyvalue].style.borderColor = "#86b7cd";

    //document.forms[0].elements[buyvalue].value = "";

    //        var cart = document.getElementById("cartqty");
    //        if (cart != null) {
    //            var cnt = cart.innerHTML;
    //            cnt = cnt.replace("(", "");
    //            cnt = cnt.replace(")", "");
    //            cnt = Number(cnt) + 1;
    //            cart.innerHTML = "(" + cnt + ")";

    //        }
    // GetCartCount();

}
function callproductqtykeypress(e) {
    var bv = document.getElementById(e.target.id);
    if (bv != null) {
        if (bv.value == "") {
            bv.style.borderColor = "red";
        }
        else {
            bv.style.borderColor = "#86b7cd";
        }
    }
}
function CallProductPopupMS(orgurl, buyvalue, pid, qtyval, tOrderID, fid) {

    var url = "";

    if (tOrderID != null && parseInt(tOrderID) > 0) {
        url = orgurl + "mOrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval.trim() + "&ORDER_ID=" + tOrderID + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

    }
    else {
        url = orgurl + "mOrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval.trim() + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

    }

    tb_show(null, url, null);

    document.forms[0].elements[buyvalue].style.borderColor = "#86b7cd";
    //var bv = document.getElementById(buyvalue);
    //bv.value="";
    //document.forms[0].elements[buyvalue].value = "";


    //        var cart = document.getElementById("cartqty");
    //        if (cart != null) {
    //            var cnt = cart.innerHTML;
    //            cnt = cnt.replace("(", "");
    //            cnt = cnt.replace(")", "");
    //            cnt = Number(cnt) + 1;
    //            cart.innerHTML = "(" + cnt + ")";

    //        }
    // GetCartCount();

}
function GetCartCount() {

    var orgurl = window.location.href;

    $.ajax({
        type: "POST",
        url: "/GblWebMethods.aspx/cartcount",
        data: "{'Strvalue':'" + orgurl + "'}",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: OncartSuccess,
        error: OncartFailure


    });

}
function OncartSuccess(result) {

    var dt;
    if (result.d != null) {

        if (result.d.substring(0, 2) == "WA") {

            dt = result.d.split("~");

            var cart = document.getElementById("navcart");
            var carttop = document.getElementById("navcart-top");
            var carttopddiv = document.getElementById("navcart-top-div");
            var cartmaindiv = document.getElementById("navcart-div");

            var cartmob = document.getElementById("cart_mop");
            if (carttop != null) {
                carttop.innerHTML = dt[0].substring(2) + "<div id='navcart-top-div' class='dropdown-menu'>" + dt[1] + "</div>";
                // carttopddiv.innerHTML = innerdata;
            }
            if (cart != null) {
                cart.innerHTML = dt[0].substring(2) + "<div id='navcart-div' class='dropdown-menu'>" + dt[1] + "</div>";
            }
            var mobcart = document.getElementById("itemsCount");
            if (mobcart != null) {
                //  var dupcnt = dt[0].substring(70);
                //  var orgprocnt = dupcnt.substring(0,2);
                // mobcart.innerHTML = orgprocnt; 
                mobcart.innerHTML = dt[2];
            }
            if (cartmob != null) {
                // cartmob.innerHTML = "<div >";
                // alert(cartmob.innerHTML);
                // alert(dt[5]);
                if ($('#cart_mop').hasClass('dropdown-menu')) {
                    cartmob.innerHTML = "<div id='cart'>" + dt[5] + "</div>";
                }
                else {
                    document.getElementById("cart_mop").className = "dropdown-menu";
                    cartmob.innerHTML = "<div id='cart'>" + dt[5] + "</div>";
                }
            }


            var itms = ""; //  document.getElementById("CartItems_head");

            if (itms != null) {


                itms.innerHTML = dt[1];
            }
            var itms1 = ""; // document.getElementById("CartItems");
            if (itms1 != null) {

                itms1.innerHTML = dt[1];
            }
        }
        else {
            dt = result.d.split(",");

            var cart = document.getElementById("cartqty");
            if (cart != null) {
                cart.innerHTML = "Items in Cart: " + dt[0] + "";
            }
            var checkout = document.getElementById("cartcheckout");
            if (checkout != null) {
                checkout.href = "/checkout.aspx?OrderID=" + dt[1] + "&ApproveOrder=Approve"
            }
            var viewcart = document.getElementById("cartviewcart");
            if (viewcart != null) {
                viewcart.href = "/orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + dt[1]
            }
        }
    }
}
function OncartFailure(result) {
    alert("failure");
}