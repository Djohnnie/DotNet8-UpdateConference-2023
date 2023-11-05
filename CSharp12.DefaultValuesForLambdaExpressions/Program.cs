


var addWithDefault =
    (int addTo = 2) => addTo + 1;


var sum =
    (params int[] numbers) => numbers.Sum();



_ = addWithDefault();

_ = addWithDefault(5);

_ = sum();

_ = sum(1, 2, 3);