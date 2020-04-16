INSERT INTO Recipes (user_id, recipe_name, recipe_description, recipe_category, instructions)
VALUES ('1', 'TestRecipe', 'Yummy meal!', 'Dessert', 'Mix custard and flan. Enjoy!'); 
SELECT SCOPE_IDENTITY();

/*
SELECT TEST: sql insert -> create compare object -> make/call dao -> datatest (expected rows, actual)
INSERT TEST: make object -> make/call dao -> SELECT (expected count, actual) | (exception fail if invalid)
UPDATE TEST: sql insert -> make/call dao using max id -> (SELECT with max id to compare values)
DELETE TEST: sql insert -> make/call dao -> SELECT (expected rows, actual)
*/



