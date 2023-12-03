use std::{fs, collections::HashMap};

pub fn main(use_examples: bool) {

    let input_path = "inputs/day1/input.txt";
    let input = fs::read_to_string(input_path)
        .expect("Should have been able to read the file");

    let example_input = 
    "1abc2
    pqr3stu8vwx
    a1b2c3d4e5f
    treb7uchet";

    //let answer_p1 = part1(example_input);
    let answer_p1 = if !use_examples {part1(&example_input)} else {part1(&input)};
    println!("{}", answer_p1);

    let example_input2 = "two1nine
    eightwothree
    abcone2threexyz
    xtwone3four
    4nineeightseven2
    zoneight234
    7pqrstsixteen";

    let answer_p2 = if use_examples {part2(&example_input2)} else {part2(&input)};
    println!("{}", answer_p2);
}

fn part1(input: &str) -> i32 {
    let parts = input.split("\n");

    let sum = parts.fold(0,  |sum, part| {

        let int_itr = part.chars().filter_map(|c| {
            c.to_string().parse::<i32>().ok()
        });

        let int_vec = int_itr.collect::<Vec<i32>>();
        return sum + int_vec.first().unwrap() * 10 + int_vec.last().unwrap();
    });

    return sum
}

fn part2(input: &str) -> i32 {
    let number_names = HashMap::from([
        ("one", 1), ("two", 2), ("three", 3), ("four", 4), ("five", 5), 
        ("six", 6), ("seven", 7), ("eight", 8), ("nine", 9)
    ]);
    let parts = input.split("\n");

    //let mut r = "";
    /// RUST IS STUPID

    // number_names.keys().fold(part, |s, &key| {
    //     let v = number_names[&key].to_string();
    //     r = s.replace(key, &v).as_str();
    //     return r;
    // });

    let sum = parts.fold(0,  |sum, part| {

        let x = part.replace("one", "o1e").replace("two", "t2o").replace("three", "t3e")
        .replace("four", "f4r").replace("five", "f5e").replace("six", "s6x")
        .replace("seven", "s7n").replace("eight", "e8t").replace("nine", "n9e");
        
        let int_itr = x.chars().filter_map(|c| {
            c.to_string().parse::<i32>().ok()
        });

        let int_vec = int_itr.collect::<Vec<i32>>();
        return sum + int_vec.first().unwrap() * 10 + int_vec.last().unwrap();
    });

    return sum;
}
