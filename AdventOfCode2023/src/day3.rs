use std::fs;
use itertools::Itertools;

pub fn main(use_examples: bool) {
    
    let example_input = 
    "467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

    let input_path = "inputs/day3/input.txt";
    let input = fs::read_to_string(input_path)
        .expect("Should have been able to read the file");

    let answer_p1 = if use_examples {part1(&example_input)} else {part1(&input)};
    println!("{}", answer_p1);
}

fn part1(input: &str) -> i32 {

    let length_of_line = input.chars().position(|c| {c =='\n'}).unwrap();
    let pre_dots = ".".repeat(length_of_line);
    let post_dots = ".".repeat(length_of_line);
    let input_string = input.to_string();
    let padded_input = format!("{pre_dots}\n{input_string}\n{post_dots}");

    let some_iter = padded_input.split("\n").into_iter();

    for (line_one, line_two, line_three) in some_iter.tuple_windows() {
        println!("{}--{}--{}", line_one, line_two, line_three);
        for (i, char) in line_two.chars().enumerate() {
            if char != '.' && char.to_string().parse::<i32>().is_err() {
                print!("{}", char)
            }
        }
    }

    return 0;
}

