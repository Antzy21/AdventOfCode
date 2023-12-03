use std::fs;

use regex::Regex;

struct GameSet {
    blue: i32,
    red: i32,
    green: i32,
}

impl GameSet {
    pub fn new(game_state_str: &str) -> Self {
        let mut red: i32 = 0;
        let mut blue: i32 = 0;
        let mut green: i32 = 0;

        for part in game_state_str.split(",") {
            match part.split_whitespace().collect::<Vec<&str>>()[..2] {
                [num, "red"] => red = num.parse::<i32>().unwrap(),
                [num, "green"] => green = num.parse::<i32>().unwrap(),
                [num, "blue"] => blue = num.parse::<i32>().unwrap(),
                _ => println!("Error parsing game state: '{}'", part)
            }
        }
        GameSet { blue, red, green }
    }

    pub fn to_string(self) -> String {
        return format!("r:{} - b:{} - g:{}", self.red, self.blue, self.green)
    }
}

struct Game {
    game_sets: Vec<GameSet>,
    game_number: i32,
    min_red: i32,
    min_green: i32,
    min_blue: i32,
}

impl Game {
    pub fn new(game_number: i32, game_set_strings: &str) -> Self {

        let game_sets = game_set_strings.to_string().split(";").into_iter().map(GameSet::new).collect::<Vec<GameSet>>();

        let min_red = game_sets.iter().map(|gs| {gs.red}).max_by(|gs1, gs2| {gs1.cmp(gs2)}).unwrap();
        let min_green = game_sets.iter().map(|gs| {gs.green}).max_by(|gs1, gs2| {gs1.cmp(gs2)}).unwrap();
        let min_blue = game_sets.iter().map(|gs| {gs.blue}).max_by(|gs1, gs2| {gs1.cmp(gs2)}).unwrap();

        Game {
            game_number: game_number,
            game_sets,
            min_red,
            min_green,
            min_blue
        }
    }

    pub fn to_string(self) -> String {
        let mut game_sets_str: String = "  ".to_owned();
        for game_set in self.game_sets {
            game_sets_str = game_sets_str + "\n  " + game_set.to_string().as_str();
        }
        return format!("Game {}: {}", self.game_number, game_sets_str);
    }
}

pub fn main(use_examples: bool) {

    let input_path = "inputs/day2/input.txt";
    let input = fs::read_to_string(input_path)
        .expect("Should have been able to read the file");

    let example_input = 
    "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
    Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
    Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
    Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
    Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";

    let answer_p1 = if use_examples {part1(&example_input)} else {part1(&input)};
    println!("{}", answer_p1);
    
    let answer_p2 = if use_examples {part2(&example_input)} else {part2(&input)};
    println!("{}", answer_p2);
}

fn parse_games(input: &str) -> Vec<Game> {
    let re = Regex::new(r"(?m)^\s*Game (\d+): (.+)$").unwrap();

    let mut games = vec![];
    for (_, [game_number, game_set_strings]) in re.captures_iter(input).map(|c| c.extract()) {
        games.push(Game::new(game_number.parse::<i32>().unwrap(), game_set_strings));
    }

    return games;
}

fn part1(input: &str) -> i32 {
    
    let games = parse_games(input);

    let min_red = 12;
    let min_green = 13;
    let min_blue = 14;

    return games.iter().filter(|g| {
       return g.game_sets.iter().all(|gs| {
            return gs.red <= min_red && gs.green <= min_green && gs.blue <= min_blue;
       }); 
    }).map(|g| {
        g.game_number
    }).sum();
}

fn part2(input: &str) -> i32 {

    let games = parse_games(input);
    
    return games.iter().map(|g| {
        g.min_red * g.min_green * g.min_blue
    }).sum();
}