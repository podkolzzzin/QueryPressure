git clean -fdx
docker build . -t query-pressure-build:latest
mkdir .out
docker run -v "C:\Users\andriipodkolzin\source\repos\review\QueryPressure\.out:/.out" query-pressure-build:latest
# todo: remove absolute path
