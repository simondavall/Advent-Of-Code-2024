INPUTS=sample1.txt sample2.txt sample3.txt input.txt

.PHONY: test

test: Program.cs $(INPUTS)
	dotnet build -c Debug && ./bin/Debug/net8.0/Day17 $(INPUTS)
