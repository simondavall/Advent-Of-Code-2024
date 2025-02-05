INPUTS=sample.txt sample2.txt input.txt

.PHONY: test

test: Program.cs $(INPUTS)
	dotnet build -c Debug && ./bin/Debug/net8.0/Day24 $(INPUTS)
