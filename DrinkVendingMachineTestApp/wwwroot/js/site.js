
function add_coin() {
    var element = document.getElementsByClassName('cheked_coin')[0];
    if (element == null) {
        alert('Вы не выбрали монету!');
        return;
    }
    var nominal = element.getAttribute('data');
    const data = { nominal: nominal };

    $.ajax(
        {
            url: '/User/AddUsersCoin',
            type: 'POST',
            data: data,
            success: function (result, status, xhr) {
                var elements = document.getElementsByClassName('cheked_coin');
                Array.from(elements).forEach(function (item) {
                    item.className = 'coin';
                });
                document.getElementById('cash_sum').innerHTML = "Кредит: " + result.toString() + " руб.";

            },
            error: function (xhr, status, error) { alert('Что-то пошло не так...') }
        });
}


function checkCoin(e) {
    var elements = document.getElementsByClassName('cheked_coin');

    Array.from(elements).forEach(function (item) {
        item.className = 'coin';
    });
    e.className = 'cheked_coin';
}


function check_drink(e) {
    var elements = document.getElementsByClassName('check_drink');
    Array.from(elements).forEach(function (item) {
        item.classList.remove('check_drink');
    });
    e.classList.add('check_drink');

    var id = e.getAttribute('data');
    const data = { id: id }

    $.ajax(
        {
            url: '/User/AddUsersDrink',
            type: 'POST',
            data: data,
            success: function (result, status, xhr) {
                //var elements = document.getElementsByClassName('check_drink');
                //Array.from(elements).forEach(function (item) {
                //    item.classList.remove('check_drink');
                //});

                document.getElementById('cost_drink').innerHTML = "Стоимость: " + result.toString() + " руб.";

            },
            error: function (xhr, status, error) { alert(error) }
        });
}

function buy_drink() {

    $.ajax(
        {
            url: '/User/BuyDrink',
            type: 'POST',

            success: function (result, status, xhr) {
                if (result == -1) {
                    alert('Недостаточно средств!');
                    return;
                }
                if (result == -2) {
                    alert('Не выбран напиток!');
                    return;
                }
                //помещаем напиток в PUSH

                document.getElementById('cash_sum').innerHTML = "Кредит: " + result.toString() + " руб.";
                let drink = document.querySelector('.check_drink .drink img');
                let push_container = document.getElementById('push');

                drink.classList.add('fall_down');
                drink.addEventListener("animationend", function () {
                    drink.classList.remove('fall_down');
                });
                document.querySelector('.check_drink').classList.remove('check_drink');

                let rotate_drink = drink.cloneNode(true);
                rotate_drink.classList.remove('fall_down');
                rotate_drink.classList.add('rotate_drink');
                push_container.addEventListener('click', function () { rotate_drink.remove(); });
                push_container.appendChild(rotate_drink);



            },
            error: function (xhr, status, error) { alert('Что-то пошло не так...') }
        });
}

function return_cash_back() {

    $.ajax(
        {
            url: '/User/ReturnCashBack',
            type: 'POST',

            success: function (result, status, xhr) {
                if (result == -1) {
                    alert('Сдачи нет!');
                    return;
                }
                document.getElementById('cash_sum').innerHTML = "Кредит:  0 руб.";
                let parent = document.getElementById('cash_back');
                for (key in result) {
                    let div = document.createElement('div');
                    div.className = 'cash_back_item';
                    let img = document.createElement('img');
                    let nominal = 0;
                    switch (key) {
                        case 'One': nominal = 1; break;
                        case 'Two': nominal = 2; break;
                        case 'Five': nominal = 5; break;
                        case 'Ten': nominal = 10; break;
                    }

                    img.src = '/images/coin/' + nominal.toString() + '.png';
                    let p = document.createElement('p');
                    p.innerHTML = 'X ' + result[key].toString() + ' шт.';
                    div.append(img);
                    div.append(p);
                    
                    parent.append(div);

                }
                let btn = document.createElement('a');
                btn.classList.add('btn');
                btn.innerHTML = 'Забрать';
                btn.addEventListener('click', function () {
                    let parent = document.getElementById('cash_back');
                    parent.innerHTML = '';
                })
                parent.append(btn);
            },
            error: function (xhr, status, error) { alert('Что-то пошло не так...') }
        });
}


function select_drink_machine(el) {
    let drink_machine_Id = el.value;
    
    $.ajax(
        {
            url: '/Admin/CheckDrinkMachine',
            type: 'POST',
            data: { id: drink_machine_Id },
            success: function (result, status, xhr) {
                let tableDrinks = document.getElementById('drinks_table');
 
                tableDrinks.innerHTML = '';

                let first_row = document.createElement('tr');
                let f_col1 = document.createElement('th');
                f_col1.innerHTML = 'Название';
                let f_col2 = document.createElement('th');
                f_col2.innerHTML = 'Стоимость';
                let f_col3 = document.createElement('th');
                f_col3.innerHTML = 'Количество';
                let f_col4 = document.createElement('th');
                f_col4.innerHTML = 'Путь к фото';


                first_row.append(f_col1);
                first_row.append(f_col2);
                first_row.append(f_col3);
                first_row.append(f_col4);
                tableDrinks.append(first_row);

                let drinks = result['drinks'];
                for (key in drinks) {

                    let drink = drinks[key];

                    let row = document.createElement('tr');
                    //row.setAttribute('data', drink['drink']['id']);

                    let col1 = document.createElement('td');
                    col1.innerHTML = drink['drink']['name'];
                    
                    let col2 = document.createElement('td');
                    col2.innerHTML = drink['drink']['price'];

                    let col3 = document.createElement('td');
                    col3.innerHTML = drink['count'];

                    let col4 = document.createElement('td');
                    col4.innerHTML = drink['drink']['imagePath'];

                    row.append(col1);
                    row.append(col2);
                    row.append(col3);
                    row.append(col4);

                    row.classList.add('hov_class');

                    let drink_id = drink['drink']['id'];
                    row.addEventListener('click', function () { click_drink_row(drink_id, drink_machine_Id); });

                    tableDrinks.append(row);
                }

                let tableCoins = document.getElementById('coins_table');
                

                tableCoins.innerHTML = '';

                let first_row_c = document.createElement('tr');
                let f_col1_c = document.createElement('th');
                f_col1_c.innerHTML = 'Номинал';
                let f_col2_c = document.createElement('th');
                f_col2_c.innerHTML = 'Количество';
                let f_col3_c = document.createElement('th');
                f_col3_c.innerHTML = 'Статус';

                first_row_c.append(f_col1_c);
                first_row_c.append(f_col2_c);
                first_row_c.append(f_col3_c);
                tableCoins.append(first_row_c);

                let coins = result['coins'];
                for (key in coins) {
                    let coin = coins[key];

                    let row = document.createElement('tr');
                    row.setAttribute('data', coin['id']);
                    let col1 = document.createElement('td');
                    col1.innerHTML = coin['coin']['denominator'];
                    let col2 = document.createElement('td');
                    col2.innerHTML = coin['count'];
                    let col3 = document.createElement('td');
                    col3.innerHTML = coin['status'] == 'false' ? 'Заблокирована' : 'Активна';

                    row.append(col1);
                    row.append(col2);
                    row.append(col3);
                    row.classList.add('hov_class');



                    tableCoins.append(row);

                }
          
            },
            error: function (xhr, status, error) { alert('Что-то пошло не так...') }
        });
}

function click_drink_row(id_drink, drink_machine_Id) {

    var drink_name = document.getElementById('drink_name_input');
    var drink_cost = document.getElementById('drink_cost_input');
    var drink_count = document.getElementById('drink_count_input');
    var drink_img = document.getElementById('drink_input_image');
    var save_btn = document.getElementById('save_btn');
    //save_btn.innerText = "Сохранить";

    $.ajax(
        {
            url: '/Admin/CheckUpdateRow',
            type: 'POST',
            data: { idDrinkMachine: drink_machine_Id, idDrink: id_drink },

            success: function (result, status, xhr) {
                let drink = result['drink'];
                let count = result['count'];

                drink_name.value = drink['name'];
                drink_cost.value = drink['price'];
                drink_count.value = count;
                drink_img.src = drink['imagePath'];

            },
            error: function (xhr, status, error) { alert('Что-то пошло не так...') }
        });

}