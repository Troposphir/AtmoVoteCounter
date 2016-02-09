# Atmosphir Vote Counter
A simple command line application to tally up votes. It uses fuzzy matching to match votes to levels so you don't have to check each one individually.

By default it looks for the levels to be voted on in `levels.txt`, and the votes in `votes.txt`. You can change this through command line arguments:

```
    AtmoVoteCounter.exe -l=comp_cc16_levels.txt -v=comp_cc16_votes.txt
```

## `levels.txt` format
`levels.txt` should contain one level entry per line, formatted exactly like this:

```
<Level Title> by <Designer>
<Level Title> by <Designer>
<Level Title> by <Designer>
etc...
```

## `votes.txt` format
`votes.txt` should have the following format:

```
/<Username>
1. <Level>
2. <Level>
3. <Level>
/<Username>
1. <Level>
2. <Level>
3. <Level>
etc...
```

The format of `<Level>` is flexible, and (as stated earlier) is fuzzy matched against the level list, so you can pretty much just copy and paste from the forums.

`<Level>` is fuzzy matched against `<Level Title>` and `<Level Title> by <Designer>`, and whichever value is higher is considered the match value for that level.