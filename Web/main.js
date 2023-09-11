const map = new ol.Map({
    target: "map",
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM(),
        }),
    ],
    view: new ol.View({
        center: [3843306.067740833, 4699999.440979294],
        zoom: 7
    }),
});

let drawing = false;
let popupPanel;

const addPointButton = document.getElementById('addPointButton');

addPointButton.addEventListener('click', () => {
    const draw = new ol.interaction.Draw({
        source: new ol.source.Vector(),
        type: 'Point'
    });

    map.addInteraction(draw);

    draw.on('drawend', (event) => {
        const coordinates = event.feature.getGeometry().getCoordinates();
        showPopup(coordinates);
        map.removeInteraction(draw);
    });

    drawing = true;
});

function showPopup(coordinates) {
    if (popupPanel) {
        popupPanel.close();
    }

    popupPanel = jsPanel.create({
        theme: 'primary',
        headerTitle: 'Add Point',
        content: `
            <div class="ma-pop-up">
            <div>
                <label class="ma-label" for="pointName">Point Name:</label>
                <input class="ma-input" type="text" id="pointName">
            </div>
            <div>
                <label class="ma-label" for="pointX">X Coordinate:</label>
                <input class="ma-input" type="text" id="pointX" readonly>
            </div>
            <div>
                <label class="ma-label" for="pointY">Y Coordinate:</label>
                <input class="ma-input" type="text" id="pointY" readonly>
            </div>
            <button class="ma-save-button" id="saveButton">Save</button>
            </div>
        `,
        contentSize: 'auto',
        position: 'center',
        animateIn: 'jsPanelFadeIn',
        animateOut: 'jsPanelFadeOut',
        callback: (panel) => {
            const pointNameInput = document.getElementById('pointName');
            const pointXInput = document.getElementById('pointX');
            const pointYInput = document.getElementById('pointY');
            const saveButton = document.getElementById('saveButton');

            pointNameInput.value = '';
            pointXInput.value = coordinates[0];
            pointYInput.value = coordinates[1];

            saveButton.addEventListener('click', () => {
                const pointName = pointNameInput.value;
                const pointX = parseFloat(pointXInput.value);
                const pointY = parseFloat(pointYInput.value);

                const data = {
                    name: pointName,
                    x: pointX,
                    y: pointY
                };



                fetch('http://localhost:5124/api/DoorApi/Add', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(data)
                    })
                    .then(response => response.json())
                    .then(result => {
                        if (result.isSuccess) {
                            showSuccessToast('Point saved successfully', 'success');
                            panel.close();
                        } else {
                            showErrorToast('Failed to save point: ' + result.message, 'error');
                        }
                    })
                    .catch(error => {
                        showErrorToast('Network error: ' + error.message, 'error');
                    });
            });
        }
    });
}

function showSuccessToast(message) {
    Toastify({
        text: message,
        duration: 3000,
        newWindow: true,
        close: true,
        gravity: 'top-right',
        position: 'right',
        backgroundColor: 'green',
        stopOnFocus: true,
    }).showToast();
}

function showErrorToast(message) {
    Toastify({
        text: message,
        duration: 3000,
        newWindow: true,
        close: true,
        gravity: 'top-right',
        position: 'right',
        backgroundColor: 'red',
        stopOnFocus: true,
    }).showToast();
}

const queryPointButton = document.getElementById('queryPointButton');

queryPointButton.addEventListener('click', () => {
    fetch('http://localhost:5124/api/DoorApi/GetAll', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        })
        .then(response => response.json())
        .then(result => {
            if (result.isSuccess) {
                const data = result.data;
                showQueryPanel(data);
            } else {
                showErrorToast('Failed to fetch points: ' + result.message, 'error');
            }
        })
        .catch(error => {
            showErrorToast('Network error: ' + error.message, 'error');
        });
});

function showQueryPanel(data) {
    if (popupPanel) {
        popupPanel.close();
    }

    popupPanel = jsPanel.create({
        theme: 'primary',
        headerTitle: 'Query Points',
        content: `
            <div class="ma-pop-up">
                <table id="pointTable" class="display">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>X Coordinate</th>
                            <th>Y Coordinate</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${generateTableRows(data)}
                    </tbody>
                </table>
            </div>
        `,
        contentSize: 'auto',
        position: 'center',
        animateIn: 'jsPanelFadeIn',
        animateOut: 'jsPanelFadeOut',
    });

    $('#pointTable').DataTable({
        paging: true,
        pageLength: 5,
    });
}

function generateTableRows(data) {
    let rows = '';
    data.forEach(point => {
        rows += `
            <tr>
                <td>${point.id}</td>
                <td>${point.name}</td>
                <td>${point.x}</td>
                <td>${point.y}</td>
            </tr>
        `;
    });
    return rows;
}

const pointsSource = new ol.source.Vector();

const pointsLayer = new ol.layer.Vector({
    source: pointsSource,
    style: new ol.style.Style({
        image: new ol.style.Circle({
            radius: 6,
            fill: new ol.style.Fill({
                color: 'purple',
            }),
        }),

    }),
});



map.addLayer(pointsLayer);

fetch('http://localhost:5124/api/DoorApi/GetAll', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    })
    .then(response => response.json())
    .then(result => {
        if (result.isSuccess) {
            const data = result.data;

            data.forEach(point => {
                const feature = new ol.Feature({
                    geometry: new ol.geom.Point([point.x, point.y]),
                    name: point.name,
                    id: point.id,
                });
                const textStyle = new ol.style.Text({
                    text: `${point.name} (${point.id})`,
                    fill: new ol.style.Fill({
                        color: 'black',
                    }),
                    offsetY: -14,
                });

                feature.setStyle(new ol.style.Style({
                    image: new ol.style.Circle({
                        radius: 6,
                        fill: new ol.style.Fill({
                            color: 'purple',
                        }),
                    }),
                    text: textStyle,
                }));
                pointsSource.addFeature(feature);
            });
        } else {
            showErrorToast('Failed to fetch points: ' + result.message, 'error');
        }
    })
    .catch(error => {
        showErrorToast('Network error: ' + error.message, 'error');
    });