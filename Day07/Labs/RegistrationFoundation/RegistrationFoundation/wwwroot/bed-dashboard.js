// ===================================================
// Hospital Bed Availability Dashboard
// Features:
// 1. Added Bed 11 and Bed 12
// 2. Grid changed to 4 columns
// 3. Occupied / Total count shown
// 4. Occupied beds cannot be clicked
// ===================================================


// -----------------------------
// BED DATA
// -----------------------------
let beds = [
    { bedNumber: 1, isOccupied: false },
    { bedNumber: 2, isOccupied: true },
    { bedNumber: 3, isOccupied: false },
    { bedNumber: 4, isOccupied: true },
    { bedNumber: 5, isOccupied: false },
    { bedNumber: 6, isOccupied: false },
    { bedNumber: 7, isOccupied: true },
    { bedNumber: 8, isOccupied: false },
    { bedNumber: 9, isOccupied: true },
    { bedNumber: 10, isOccupied: false },

    // Added beds
    { bedNumber: 11, isOccupied: false },
    { bedNumber: 12, isOccupied: false }
];


// -----------------------------
// FUNCTION: Update Counter
// -----------------------------
function updateBedCount() {

    let occupiedCount = 0;

    for (let i = 0; i < beds.length; i++) {
        if (beds[i].isOccupied) {
            occupiedCount++;
        }
    }

    document.getElementById("bedCount").innerText =
        occupiedCount + "/" + beds.length;
}


// -----------------------------
// FUNCTION: Render Beds
// -----------------------------
function renderBeds() {

    let container = document.getElementById("bedContainer");

    container.innerHTML = "";

    for (let i = 0; i < beds.length; i++) {

        let bed = beds[i];

        let bedDiv = document.createElement("div");

        bedDiv.classList.add("bed");

        if (bed.isOccupied) {

            bedDiv.classList.add("occupied");

            bedDiv.innerHTML =
                `Bed ${bed.bedNumber}<br>Occupied`;

            // Occupied beds cannot be clicked
            bedDiv.onclick = null;

        } else {

            bedDiv.classList.add("available");

            bedDiv.innerHTML =
                `Bed ${bed.bedNumber}<br>Available`;

            // Only available beds can be occupied
            bedDiv.onclick = function () {

                bed.isOccupied = true;

                renderBeds();
            };
        }

        container.appendChild(bedDiv);
    }

    updateBedCount();
}


// -----------------------------
// INITIAL LOAD
// -----------------------------
renderBeds();