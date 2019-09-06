var homeShopViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.items = ko.observableArray();
        self.addItemToCartClick = onAddItemToCartClick;
    };

    homeShopViewModel = new createViewModel();

    ko.applyBindings(homeShopViewModel, document.getElementById("homeShopContainer"));   

    loadPopularItems();

    recalculatePopularItemsContianerHeight();

    $(window).resize(() => recalculatePopularItemsContianerHeight());
});

function recalculatePopularItemsContianerHeight() {
    const pageContainerOffsetTop = $('#homeShopContainer').offset().top;
    const categoryContainerHeight = $('.categories-container').outerHeight(true);

    const itemsContainerHeight = $(window).height() - pageContainerOffsetTop - categoryContainerHeight- 7;

    $('.popular-items-container').height(itemsContainerHeight + 'px');
}

function loadPopularItems() {

    var loadItemsPromise = $.ajax({
        type: "GET",
        url: '/items/api/get-most-popular-items',
        contentType: "application/json",
        dataType: "json"
    });

    loadItemsPromise.done(function (result) {
        var items = result.map(item => new Item(item));
        //var items = [];
        //items.push(new Item(result[0]));
        ko.utils.arrayPushAll(homeShopViewModel.items, items);
        ko.utils.arrayPushAll(homeShopViewModel.items, items);
        ko.utils.arrayPushAll(homeShopViewModel.items, items);
        animateItems();
    });

    loadItemsPromise.fail(function () {
        alert("error!");
    });
}

function animateItems() {
    var container = $('.popular-items-container');
    var items = $('.item');
    var currrentZIndex = 1;

    var widthTranslation = container.width() / 2 - items.first().width() / 2;
    var heightTranslation = container.height() / 2 - items.first().height() / 2;

    $(items.get().reverse()).each(function (index) {
        const yTranslationValue = getRandomNumber(heightTranslation * -1, heightTranslation);
        TweenLite.to(this, .5, {
            delay: index * 0.1,
            x: getRandomNumber(widthTranslation * -1, widthTranslation),
            y: (yTranslationValue < 0 && heightTranslation + yTranslationValue < 50) ? heightTranslation - 50 : yTranslationValue,
            rotation: getRandomNumber(-30, 30)
        });
    });

    Draggable.create(items, {
        bounds: container,
        edgeResistance: 0,
        type: 'x,y',
        zIndexBoost: false,
        onClick: function (e) {
            var $this = $(this.target);
            TweenLite.set($this, { cursor: "pointer" });
            $this.css('z-index', ++currrentZIndex);
            if ($this.hasClass('active')) {
                if ($(e.target).hasClass('ns-btn')) {
                    console.log('sssss');
                    e.stopPropagation();
                    return;
                }
                window.location.href = $this.attr('data-url');

                //TweenLite.to($this, .2, {
                //    scaleX: 1,
                //    scaleY: 1,
                //    x: $this.attr('data-x'),
                //    y: $this.attr('data-y'),
                //    rotation: $this.attr('data-r')
                //});
                //$this.removeClass('active');
            } else {
                $this.attr('data-x', $this[0]._gsTransform.x).attr('data-y', $this[0]._gsTransform.y).attr('data-r', $this[0]._gsTransform.rotation);
                TweenLite.to($this, .2, {
                    scaleX: 2,
                    scaleY: 2,
                    x: 0,
                    y: 0,
                    rotation: 0
                });
                TweenLite.to($('.item.active'), .2, {
                    scaleX: 1,
                    scaleY: 1,
                    x: $('.item.active').attr('data-x'),
                    y: $('.item.active').attr('data-y'),
                    rotation: $('.item.active').attr('data-r')
                });
                TweenLite.set($('.item.active'), { cursor: "move" });
                $('.item.active').removeClass('active');
                $this.addClass('active');
            }
        },
        onPress: function () {
            var $this = $(this.target);
            $this.css('z-index', ++currrentZIndex);
            $('.item.active').css('z-index', ++currrentZIndex);
        },
        onDrag: function () {
            var $this = $(this.target);
            TweenLite.to($this, .2, {
                scaleX: 1,
                scaleY: 1
            });
            $this.removeClass('active');
        }
    });
}

function getRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

$('.popular-items-container').click(function (event) {
    if ($(event.target).hasClass('popular-items-container')) {
        TweenLite.to($('.active'), .2, {
            scaleX: 1,
            scaleY: 1,
            x: $('.active').attr('data-x'),
            y: $('.active').attr('data-y'),
            rotation: $('.active').attr('data-r')
        });

        TweenLite.set($('.item.active'), { cursor: "move" });
        $('.item.active').removeClass('active');
    }
})

function Item(item) {
    this.Code = item.Code;
    this.Price = item.Price;
    this.Name = item.Name;
    this.MainImageName = item.MainImageName;
}

function onAddItemToCartClick(item, e) {
    addItemToShoppingCart(item.Code, item.Price, item.Name, item.MainImageName);
}
