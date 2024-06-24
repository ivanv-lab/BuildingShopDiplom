$.ajax({
    url: '/Home/GetChartData',
    type: 'GET',
    dataType: 'json',
    success: function (data) {
        console.log("success");
        let labels = [];
        let dataset = [];
        for (var item in data) {

            var year = data[item].year;
            var month = data[item].month;
            if (month < 10) {
                month = '0' + month.toString();
            }
            var formattedDate = `${month}.${year}`;
            labels.push(formattedDate);
            dataset.push(data[item].sum);
        }

        let ctx = document.getElementById("myChart").getContext('2d');
        let chart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Сумма заказов',
                    data: dataset,
                    borderColor: 'rgba(18, 80, 255, 0.8)',
                    backgroundColor: 'rgba(18, 80, 255, 0.3)',
                    fill: true
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true,
                        stepSize: 2
                    }
                }
            }
        });
        $('.loader').fadeOut();
    },
    error: function (jqXHR, textStatus, errorThrown) {
        console.error(textStatus, errorThrown);
    }
});