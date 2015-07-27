$.datepicker.regional['lt-LT'] = {
    closeText: 'Uždaryti',
    prevText: '&#x3c;Atgal',
    nextText: 'Pirmyn&#x3e;',
    currentText: 'Šiandien',
    monthNames: ['Sausis', 'Vasaris', 'Kovas', 'Balandis', 'Gegužė', 'Birželis',
    'Liepa', 'Rugpjūtis', 'Rugsėjis', 'Spalis', 'Lapkritis', 'Gruodis'],
    monthNamesShort: ['Sau', 'Vas', 'Kov', 'Bal', 'Geg', 'Bir',
    'Lie', 'Rugp', 'Rugs', 'Spa', 'Lap', 'Gru'],
    dayNames: ['sekmadienis', 'pirmadienis', 'antradienis', 'trečiadienis', 'ketvirtadienis', 'penktadienis', 'šeštadienis'],
    dayNamesShort: ['sek', 'pir', 'ant', 'tre', 'ket', 'pen', 'šeš'],
    dayNamesMin: ['Se', 'Pr', 'An', 'Tr', 'Ke', 'Pe', 'Še'],
    weekHeader: 'Wk',
    dateFormat: 'yy.mm.dd',
    firstDay: 1,
    isRTL: false,
    showMonthAfterYear: false,
    yearSuffix: ''
};

$.timepicker.regional['lt-LT'] = {
    timeOnlyTitle: 'Pasirinkite laiką',
    timeText: 'Laikas',
    hourText: 'Valandos',
    minuteText: 'Minutės',
    secondText: 'Sekundės',
    millisecText: 'Milisekundės',
    microsecText: 'Mikrosekundės',
    timezoneText: 'Laiko zona',
    currentText: 'Dabar',
    closeText: 'Uždaryti',
    timeFormat: 'HH:mm',
    amNames: ['priešpiet', 'AM', 'A'],
    pmNames: ['popiet', 'PM', 'P'],
    isRTL: false
};

$.datepicker.setDefaults($.datepicker.regional['lt-LT']);
$.timepicker.setDefaults($.timepicker.regional['lt-LT']);