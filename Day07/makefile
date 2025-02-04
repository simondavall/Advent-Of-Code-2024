INPUTS=sample.txt input.txt

.PHONY: test

test: Program.cs $(INPUTS)
	dotnet build -c Debug && ./bin/Debug/net8.0/Day07 $(INPUTS)
