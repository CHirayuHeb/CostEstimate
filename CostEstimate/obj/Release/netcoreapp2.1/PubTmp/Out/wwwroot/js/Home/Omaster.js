(function () {
    var activeType = "";
    var search = document.getElementById("searchInputtbOmaster");
    var tabs = document.getElementById("mmTabs");
    var totalEl = document.getElementById("mmTotal");
    var emptyEl = document.getElementById("mmEmpty");

    function apply() {
        var q = search ? (search.value || "").toLowerCase().trim() : "";
        var total = 0;

        document.querySelectorAll("#mmList .mm-group").forEach(function (g) {
            var typeOk = !activeType || g.dataset.type === activeType;
            var shown = 0;

            g.querySelectorAll(".mm-row").forEach(function (r) {
                var ok = typeOk && (!q || r.dataset.name.indexOf(q) > -1);
                r.style.display = ok ? "" : "none";
                if (ok) { shown++; r.querySelector(".mm-no").textContent = shown; }
            });

            g.style.display = shown ? "" : "none";
            g.querySelector(".mm-gc").textContent = shown;
            total += shown;
        });

        if (totalEl) totalEl.textContent = total;
        if (emptyEl) emptyEl.style.display = total ? "none" : "block";
    }

    if (tabs) {
        tabs.addEventListener("click", function (e) {
            var b = e.target.closest(".mm-tab");
            if (!b) return;
            activeType = b.dataset.type;
            tabs.querySelectorAll(".mm-tab").forEach(function (t) {
                t.classList.toggle("active", t === b);
            });
            apply();
        });
    }

    if (search) search.addEventListener("input", apply);

    apply(); // รันตอนโหลดหน้า
})();