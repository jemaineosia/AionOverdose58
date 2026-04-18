// reCAPTCHA v3 helpers for Aion Overdose 58
// window.__recaptchaSiteKey is set by App.razor from server config.

window.executeRecaptcha = function (action) {
    return new Promise(function (resolve, reject) {
        var key = window.__recaptchaSiteKey;
        if (!key) {
            // Not configured — resolve with empty string so server skips verification
            resolve('');
            return;
        }

        // Hard deadline: if the whole process takes > 4 s, give up with empty token.
        var deadline = setTimeout(function () { resolve(''); }, 4000);

        // Poll until grecaptcha is available (async defer can lose the race)
        var attempts = 0;
        var maxAttempts = 20; // 2 seconds max wait (20 × 100ms)
        var poll = function () {
            if (typeof grecaptcha !== 'undefined') {
                grecaptcha.ready(function () {
                    grecaptcha.execute(key, { action: action })
                        .then(function (token) { clearTimeout(deadline); resolve(token); })
                        .catch(function () { clearTimeout(deadline); resolve(''); });
                });
            } else if (attempts++ < maxAttempts) {
                setTimeout(poll, 100);
            } else {
                clearTimeout(deadline);
                resolve(''); // API never loaded — proceed without token
            }
        };
        poll();
    });
};

// Spinner SVG (Tailwind animate-spin) shown while awaiting reCAPTCHA token.
var _spinnerHtml =
    '<span style="display:inline-flex;align-items:center;gap:8px;">' +
    '<svg class="animate-spin" style="width:16px;height:16px;flex-shrink:0;" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">' +
    '<circle style="opacity:.25;" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>' +
    '<path style="opacity:.75;" fill="currentColor" d="M4 12a8 8 0 018-8v8z"></path>' +
    '</svg>' +
    'Logging in...' +
    '</span>';

// Wire up the SSR login form — called inline from the submit button onclick.
window.submitLoginWithRecaptcha = async function (btn) {
    var form = btn.closest('form');
    var tokenInput = document.getElementById('recaptcha-login-token');

    // Show loading state immediately so the user knows something is happening.
    var originalHtml = btn.innerHTML;
    btn.disabled = true;
    btn.innerHTML = _spinnerHtml;

    try {
        if (tokenInput) {
            tokenInput.value = await window.executeRecaptcha('login');
        }
    } catch (e) {
        console.warn('[reCAPTCHA] token fetch failed, submitting without token:', e);
        if (tokenInput) tokenInput.value = '';
    }

    // form.submit() triggers a full-page POST — the spinner stays visible until
    // the browser navigates to the response page, which is the desired UX.
    form.submit();
};
