# Concepts

## Models

* Label
  * Name:string
  * Description:string
  * Color:string
* Repository
  * Name
  * Id
  * Labels:Label[]

## Target Labels

The labels you want in each repo.

## Cases

* What happens when a repo has a label with the same name as a target label?
  * Prompts the user if he or she wants to replace the label with the target description and color.