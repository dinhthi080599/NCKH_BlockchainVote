//DOM elements
const DOMstrings = {
    stepsBtnClass: 'multisteps-form__progress-btn',
    stepsBtns: document.querySelectorAll(`.multisteps-form__progress-btn`),
    stepsBar: document.querySelector('.multisteps-form__progress'),
    stepsForm: document.querySelector('.multisteps-form__form'),
    stepsFormTextareas: document.querySelectorAll('.multisteps-form__textarea'),
    stepFormPanelClass: 'multisteps-form__panel',
    stepFormPanels: document.querySelectorAll('.multisteps-form__panel'),
    stepPrevBtnClass: 'js-btn-prev',
    stepNextBtnClass: 'js-btn-next'
};


//remove class from a set of items
const removeClasses = (elemSet, className) => {

    elemSet.forEach(elem => {

        elem.classList.remove(className);

    });

};

//return exect parent node of the element
const findParent = (elem, parentClass) => {

    let currentNode = elem;

    while (!currentNode.classList.contains(parentClass)) {
        currentNode = currentNode.parentNode;
    }

    return currentNode;

};

//get active button step number
const getActiveStep = elem => {
    return Array.from(DOMstrings.stepsBtns).indexOf(elem);
};

//set all steps before clicked (and clicked too) to active
const setActiveStep = activeStepNum => {

    //remove active state from all the state
    removeClasses(DOMstrings.stepsBtns, 'js-active');

    //set picked items to active
    DOMstrings.stepsBtns.forEach((elem, index) => {

        if (index <= activeStepNum) {
            elem.classList.add('js-active');
        }

    });
};

//get active panel
const getActivePanel = () => {

    let activePanel;

    DOMstrings.stepFormPanels.forEach(elem => {

        if (elem.classList.contains('js-active')) {

            activePanel = elem;

        }

    });

    return activePanel;

};

//open active panel (and close unactive panels)
const setActivePanel = activePanelNum => {

    //remove active class from all the panels
    removeClasses(DOMstrings.stepFormPanels, 'js-active');

    //show active panel
    DOMstrings.stepFormPanels.forEach((elem, index) => {
        if (index === activePanelNum) {

            elem.classList.add('js-active');

            setFormHeight(elem);

        }
    });

};

//set form height equal to current panel height
const formHeight = activePanel => {

    const activePanelHeight = activePanel.offsetHeight;

    DOMstrings.stepsForm.style.height = `${activePanelHeight}px`;

};

const setFormHeight = () => {
    const activePanel = getActivePanel();

    formHeight(activePanel);
};

//STEPS BAR CLICK FUNCTION
DOMstrings.stepsBar.addEventListener('click', e => {

    //check if click target is a step button
    const eventTarget = e.target;

    if (!eventTarget.classList.contains(`${DOMstrings.stepsBtnClass}`)) {
        return;
    }

    //get active button step number
    const activeStep = getActiveStep(eventTarget);

    //set all steps before clicked (and clicked too) to active
    setActiveStep(activeStep);

    //open active panel
    setActivePanel(activeStep);
});

//PREV/NEXT BTNS CLICK
DOMstrings.stepsForm.addEventListener('click', e => {

    const eventTarget = e.target;

    //check if we clicked on `PREV` or NEXT` buttons
    if (!(eventTarget.classList.contains(`${DOMstrings.stepPrevBtnClass}`) || eventTarget.classList.contains(`${DOMstrings.stepNextBtnClass}`))) {
        return;
    }

    //find active panel
    const activePanel = findParent(eventTarget, `${DOMstrings.stepFormPanelClass}`);

    let activePanelNum = Array.from(DOMstrings.stepFormPanels).indexOf(activePanel);

    //set active step and active panel onclick
    if (eventTarget.classList.contains(`${DOMstrings.stepPrevBtnClass}`)) {
        activePanelNum--;

    } else {

        activePanelNum++;

    }

    setActiveStep(activePanelNum);
    setActivePanel(activePanelNum);

});

//SETTING PROPER FORM HEIGHT ONLOAD
window.addEventListener('load', setFormHeight, false);

//SETTING PROPER FORM HEIGHT ONRESIZE
window.addEventListener('resize', setFormHeight, false);

//changing animation via animation select !!!YOU DON'T NEED THIS CODE (if you want to change animation type, just change form panels data-attr)

const setAnimationType = newType => {
    DOMstrings.stepFormPanels.forEach(elem => {
        elem.dataset.animation = newType;
    });
};

//selector onchange - changing animation
//const animationSelect = document.querySelector('.pick-animation__select');

//animationSelect.addEventListener('change', () => {
//    const newAnimationType = animationSelect.value;

//    setAnimationType(newAnimationType);
//});

$(document).ready(function () {
    $("#file-input").on("change", function () {
        var input = this
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('.avata').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }); 


    $('.datepicker_1').daterangepicker({
        "singleDatePicker": true,
        "timePicker": false,
        "drops": "up",
        "locale": {
            "autoApply": true,
            "format": "DD/MM/YYYY",
            "applyLabel": "Áp dụng",
            "cancelLabel": "Hủy",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Custom",
            "daysOfWeek": [
                "CN",
                "T2",
                "T3",
                "T4",
                "T5",
                "T6",
                "T7"
            ],
            "monthNames": [
                "Tháng 1",
                "Tháng 2",
                "Tháng 3",
                "Tháng 4",
                "Tháng 5",
                "Tháng 6",
                "Tháng 7",
                "Tháng 8",
                "Tháng 9",
                "Tháng 10",
                "Tháng 11",
                "Tháng 12",
            ],
            "firstDay": 1
        }
    });
    $('.datepicker').daterangepicker({
        "singleDatePicker": true,
        "timePicker": true,
        "timePicker24Hour": true,
        "drops": "up",
        "locale": {
            "autoApply": true,
            "format": "DD/MM/YYYY HH:mm",
            "applyLabel": "Áp dụng",
            "cancelLabel": "Hủy",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Custom",
            "daysOfWeek": [
                "CN",
                "T2",
                "T3",
                "T4",
                "T5",
                "T6",
                "T7"
            ],
            "monthNames": [
                "Tháng 1",
                "Tháng 2",
                "Tháng 3",
                "Tháng 4",
                "Tháng 5",
                "Tháng 6",
                "Tháng 7",
                "Tháng 8",
                "Tháng 9",
                "Tháng 10",
                "Tháng 11",
                "Tháng 12",
            ],
            "firstDay": 1
        }
    });

    $('.datepicker').val('');

    function count_btn_next_enabled() {
        var count_btn_next = $('button[name="btn_next"]:enabled').length;
        var btn_next_title = $('button[name="btn_next_title"]');
        for (let i = 0; i < btn_next_title.length; i++) {
            let tt;
            if (i <= count_btn_next) {
                tt = false;
            }
            else {
                tt = true;
            }
            $(btn_next_title[i]).prop('disabled', tt);
        }
    }
    
    $(document).on('click', 'input[name="type_vote"]', function () {
        if ($('input[name="type_vote"]').val() != "") {
            $(this).parent().parent().parent().parent().parent().find('.d-flex').find('.js-btn-next').prop('disabled', false);
        }
        else {
            $(this).parent().parent().parent().parent().parent().find('.d-flex').find('.js-btn-next').prop('disabled', true);
        }
        count_btn_next_enabled();
    });
    $(document).on('click', 'input[name="amount_vote"]', function () {
        if ($('input[name="type_vote"]').val() != "") {
            $(this).parent().parent().parent().parent().parent().find('.d-flex').find('.js-btn-next').prop('disabled', false);
        }
        else {
            $(this).parent().parent().parent().parent().parent().find('.d-flex').find('.js-btn-next').prop('disabled', true);
        }
        count_btn_next_enabled();
    });
    $(document).on('keyup', 'input[name="information"]', function () {
        var tt = false;
        var information = $('input[name="information"]');
        for (let i = 0; i < information.length; i++) {
            if ($(information[i]).val() == "") {
                tt = true;
                break;
            }
        }
        $(this).parent().parent().parent().parent().find('.d-flex').find('.js-btn-next').prop('disabled', tt);
        count_btn_next_enabled();
    });
    $(document).on('keyup', 'input[name="cutri"]', function () {
        var tt = false;
        var information = $('input[name="information"]');
        for (let i = 0; i < information.length; i++) {
            if ($(information[i]).val() == "") {
                tt = true;
                break;
            }
        }
        $(this).parent().parent().parent().parent().find('.d-flex').find('.js-btn-next').prop('disabled', tt);
        count_btn_next_enabled();
    });
    $(document).on('keyup', '.content_election', function () {
        var tt = false;
        if ($('.content_election').val() == "") {
            tt = true;
        }
        $('#xac_nhan').prop('disabled', tt);
        count_btn_next_enabled();
    });

    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });

    $(document).on('click', '#addElector', function () {
        var name, sex, birthday;
        name = $('#newElectorName').val();
        sex = $('#newElectorGender').children("option:selected").val();
        birthday = $('#newElectorBirthDay').val();
        var html = `
            <div class="col-12 col-md-4 ungvien">
                <div class="card shadow-sm">
                    <div class="row">
                        <div class="block col-12 pl-4 pt-4">
                            <label class="pt-2 name" style="font-size: 20px;">`+name+`</label>
                            <p class="pt-1">
                                <i class="sex">` + sex + `</i>
                                <br />
                                <i class="birthday">` + birthday + `</i>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        `;
        $(".ds_ungvien").prepend(html);
        if ($(".ungvien").length > 0) {
        };
        $('#exampleModal').modal('hide');
    });

    $(document).on('click', '#xac_nhan', function () {
        var data = {
            'ten_dot': $('#name_election').val(),
            'thoigian_bd': $('#time_start').val(),
            'thoigian_kt': $('#time_end').val(),
            'noidung': $('.content_election').val(),
            'sophieu': $('input[name="amount_vote"]:checked').val(),
        }
        $.ajax({
            url: "/CreateElection/them_dotbaucu",
            type: 'POST',
            data: data,
            success: function (result) {
                if (result != "them_thatbai") {
                    var list_ungvien = $('.ungvien');
                    var array_ungvien = [];
                    for (let i = 0; i < list_ungvien.length; i++) {
                        array_ungvien[i] = {
                            'sHoTen': $(list_ungvien[i]).find('.name').text(),
                            'bGioiTinh': $(list_ungvien[i]).find('.sex').text() == "Nam" ? true : false,
                            'dNgaySinh': $(list_ungvien[i]).find('.birthday').text(),
                            'ma_dotbaucu': result,
                        }
                    }
                    var array_ungvien = JSON.stringify({ 'array_ungvien': array_ungvien });
                    $.ajax({
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        type: 'POST',
                        url: "/CreateElection/them_ungvien_dbc",
                        data: array_ungvien,
                    });
                    Swal.fire({
                        ion: 'succes',
                        title: 'Thành công',
                        text: 'Chúc mừng, bạn đã thêm đợt bầu cử thành công!',
                    })
                }
            }
        });
    });
});





