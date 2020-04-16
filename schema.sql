

USE master;
GO


IF EXISTS(select * from sys.databases where name='MealPlanner')
DROP DATABASE MealPlanner;
GO


CREATE DATABASE MealPlanner;
GO


USE MealPlanner;
GO

BEGIN TRANSACTION;

CREATE TABLE users
(
	id			int			identity(1,1),
	username	varchar(50)	not null,
	password	varchar(50)	not null,
	salt		varchar(50)	not null,
	role		varchar(50)	default('user'),

	constraint pk_users primary key (id)
);

CREATE TABLE Ingredients
(
	id int identity primary key,
	ingredient_name varchar(50) not null,

);

CREATE TABLE Recipes
(
	id int identity primary key,
	user_id int not null,
	recipe_name  varchar(100) not null,
	recipe_description varchar(2500) not null,
	recipe_category varchar(50) not null,
	instructions varchar(2500) not null,
    FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE Recipe_Ingredients
(
	recipe_id int not null,
	ingredient_id int not null,
	ingredient_quantity varchar (7) not null,
	measurement_form varchar(50) not null,
	FOREIGN KEY (ingredient_id) REFERENCES Ingredients(id),
	FOREIGN KEY (recipe_id) REFERENCES Recipes(id)
);

CREATE TABLE meal_plans
(
	id int identity primary key,
	plan_name varchar(50) not null,
	user_id int not null,
	FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE meals
(
	id int identity primary key,
	plan_id int not null,
	meal_order int not null,
	recipe_id int,
	FOREIGN KEY (recipe_id) REFERENCES Recipes(id),
	FOREIGN KEY (plan_id) REFERENCES meal_plans(id)
);



INSERT INTO Ingredients (ingredient_name)
VALUES ('Salt'),('Pepper'),('Potato'),('Egg'),('Chicken'),('Green Pepper'),('Onion'),('Cheddar Cheese'),('Milk'),('Garlic'),('Butter'),('Asparagus'),('Sour Cream'),('Hot Sauce')

INSERT INTO users (username, password, salt, role) 
VALUES ('Aloha', '5k4i84k4l39', '4-794=5&f', 'User')
 
INSERT INTO Recipes (user_id, recipe_name, recipe_description, recipe_category, instructions)
VALUES ('1', 'Eggs and Hash', 'Cheesy Egg Hash… This is a meal when you need something quick, filling and absolutely incredibly delicious!', 'Breakfast', 'Stove Top:
Melt butter in a large cast iron skillet or pan over medium heat. Fry the potatoes in the butter and cook while stirring occasionally, until golden and crispy (about 20 minutes). To speed up cooking time, cover pan with a lid, checking the potatoes every 4-5 minutes or so to stir them (this takes about 15 minutes).
Add green pepper pieces to the pan and fry while stirring occasionally for 10 minutes until crisp. The potatoes will be golden with crisp edges, while soft on the inside. Add the onions; stir them through and season with salt and pepper (optional).
Using a wooden spoon or spatula, make four wells in the hash, crack an egg into each well and arrange the cheddar cheese around each egg. Fry until the whites are set and the eggs are cooked to your liking.
Serve immediately.')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('1', '1', '2','tbsp')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('1', '2', '1','tbsp')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('1', '3', '1','whole')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('1', '4', '2','whole')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('1', '6', '1','whole')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('1', '7', '1','whole')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('1', '8', '1','cup')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('1', '11', '1','tbsp')


INSERT INTO Recipes (user_id, recipe_name, recipe_description, recipe_category, instructions)
VALUES ('1', 'Chicken and Fries','Delicious meal that is quick, easy to make and enjoyable!','Lunch','For the chicken: Preheat the oven to 425 degrees F. Combine the salt, pepper, garlic for seasoning. Reserve 2 tablespoons of the seasoning for the fries. Pat the chicken dry with paper towels. Liberally sprinkle some of the remaining seasoning onto the chicken. Brush the outside of the chicken with the melted butter and sprinkle with the rest of the remaining seasoning, Roast the chicken until it reaches 165 degrees F, about 1 1/2 hours. Remove the chicken and cover with aluminum foil for about 20 minutes.
For the fries: Preheat the oven to 450 degrees. In a large bowl, toss the potatoes with 1/2 teaspoon salt. Coat a baking sheet with cooking butter and spread the potatoes in a single layer after cutting into shoestring form(optional). Bake until golden and crisp, about 35 minutes. Remove the fries with a spatula and season with salt.
Serve the chicken with the fries. Enjoy!')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('2', '1', '3','tbsp')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('2', '2', '2','tbsp')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('2', '3', '3','whole')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('2', '5', '1','breast')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('2', '10', '3','tbsp')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('2', '11', '5','tbsp')


INSERT INTO Recipes (user_id, recipe_name, recipe_description, recipe_category, instructions)
VALUES ('1', 'Chicken with Garlic Potatoes and Asparagus', 'Hearty dinner the whole family will love! Incorporates old favorites with new bold flavors.', 'Dinner', 'Put a rimmed baking sheet on the lowest oven rack and preheat to 425 degrees F. Toss the potatoes and garlic cloves with 1 tablespoon butter, 1/4 teaspoon salt and pepper in a bowl; set aside. Season the chicken with 1/2 teaspoon salt and pepper. Heat 1 tablespoon butter in a large skillet over medium heat. Add the chicken; cook until browned, 1 1/2 to 2 minutes per side. Transfer to the 
hot baking sheet. Scatter the potatoes and garlic around the chicken; reserve the bowl. Bake until the chicken is just cooked through, about 5 minutes. Transfer the chicken to a plate and tent with foil; leave the potatoes and garlic on the baking sheet.Add the asparagus to the reserved bowl; toss with the remaining 1 tablespoon butter and 1/4 teaspoon salt. Add to the baking sheet with the potatoes. Bake until the vegetables are tender up to 13 to 15 minutes.Remove the baking sheet from the oven; switch the oven to broil.Nestle the chicken in the vegetables and sprinkle everything with lemon zest. Broil until golden, 1 to 2 minutes.')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('3', '1', '3','tsp')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('3', '2', '2','tsp')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('3', '3', '2','whole')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('3', '5', '3','breast')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('3', '10', '3','cloves')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('3', '11', '5','tbsp')

INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, ingredient_quantity, measurement_form)
VALUES ('3', '12', '1','bunch')

 COMMIT TRANSACTION;
