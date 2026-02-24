SOLUTION = ScreenTask.sln
EXE      = ScreenTask/bin/Release/ScreenTask.exe

.PHONY: build debug clean run format

## build release binary (default)
build:
	msbuild $(SOLUTION) /p:Configuration=Release /v:minimal

## build debug binary
debug:
	msbuild $(SOLUTION) /p:Configuration=Debug /v:minimal

## clean all build outputs
clean:
	msbuild $(SOLUTION) /t:Clean /v:minimal

## build release then launch (requires admin - UAC prompt will appear)
run: build
	$(EXE)

## run dotnet format
format:
	dotnet format $(SOLUTION) --verbosity diagnostic
