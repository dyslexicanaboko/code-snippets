#!/bin/sh
# Modified from source: https://gist.github.com/bjorng/154761
# This script does not determine which branches are stale, it just prints what exists on the remote essentially
# Branches are considered stale when their last commit is older than 3 months
# Good idea to make sure your local is clean and you have latest before running this
# 
# Using cmder - run `sh gitStaleBranches.sh` and it will output branches one per line
# If you want to save it to a file run `sh gitStaleBranches.sh > [path to file]`
IFS='
'

# Name of remote repository. Can be edited.
remote=origin

for i in `git branch -r | grep "^ *$remote/" | grep -v HEAD | sed "s;^ *$remote/;;"`
do
	if git rev-parse -q --verify $i >/dev/null
	then
	   nothing=
	else
	   printf "%s\r\n" "$i"
	fi
done
