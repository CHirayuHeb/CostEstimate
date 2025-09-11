document.querySelectorAll(".accordion-header").forEach(header => {
    header.addEventListener("click", () => {
        const content = header.nextElementSibling;
        const icon = header.querySelector(".toggle-icon");

        // toggle แสดง/ซ่อน
        if (content.style.display === "block") {
            content.style.display = "none";
            icon.textContent = "+";
        } else {
            content.style.display = "block";
            icon.textContent = "-";
        }
    });
});



function bindTableSearch(inputId, tableClass) {
    const input = document.querySelector(`#${inputId}`);
    if (!input) return; // ป้องกันกรณี element ไม่มี

    input.addEventListener("keyup", () => {
        const filter = input.value.toLowerCase();
        const rows = document.querySelectorAll(`.${tableClass} tbody tr`);

        rows.forEach(row => {
            const cells = row.querySelectorAll("td");
            let match = false;

            cells.forEach(cell => {
                if (cell.textContent.toLowerCase().includes(filter)) {
                    match = true;
                }
            });

            row.style.display = match ? "" : "none";
        });
    });
}

// bind ทุก table
bindTableSearch("searchInputtbOther", "tbOther");
bindTableSearch("searchInputtbWK", "tbWK");
bindTableSearch("searchInputtbMT", "tbMT");
bindTableSearch("searchInputtbTGR", "tbTGR");
bindTableSearch("searchInputtbSM", "tbSM");