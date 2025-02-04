INPUTS=input.txt

.PHONY: test

test: Program.cs $(INPUTS)
	dotnet build -c Debug && ./bin/Debug/net8.0/Day14 $(INPUTS)
