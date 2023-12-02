/// <summary>
/// 
/// </summary>
namespace UniversalOptimizer.Opt.SingleObjective.Teaching
{

    using sys;

    using Path = pathlib.Path;

    using randrange = random.randrange;

    using seed = random.seed;

    using datetime = datetime.datetime;

    using BitArray = bitstring.BitArray;

    using xr = xarray;

    using Model = linopy.Model;

    using OutputControl = uo.Algorithm.OutputControl.OutputControl;

    using FinishControl = uo.Algorithm.Metaheuristic.finishControl.FinishControl;

    using AdditionalStatisticsControl = uo.Algorithm.Metaheuristic.additionalStatisticsControl.AdditionalStatisticsControl;

    using TeOptimizerConstructionParameters = uo.Algorithm.Exact.TotalEnumeration.te_optimizer.TeOptimizerConstructionParameters;

    using VnsOptimizerConstructionParameters = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.vns_optimizer.VnsOptimizerConstructionParameters;

    using ensure_dir = uo.utils.files.ensure_dir;

    using logger = uo.utils.logger.logger;

    using default_parameters_cl = Teaching.FunctionOneVariableProblem.command_line.default_parameters_cl;

    using parse_arguments = Teaching.FunctionOneVariableProblem.command_line.parse_arguments;

    using FunctionOneVariableProblem = Teaching.FunctionOneVariableProblem.FunctionOneVariableProblem.FunctionOneVariableProblem;

    using FunctionOneVariableProblemBinaryIntSolution = Teaching.FunctionOneVariableProblem.FunctionOneVariableProblemBinaryIntSolution.FunctionOneVariableProblemBinaryIntSolution;

    using FunctionOneVariableProblemBinaryIntSolutionVnsSupport = Teaching.FunctionOneVariableProblem.FunctionOneVariableProblemBinaryIntSolutionVnsSupport.FunctionOneVariableProblemBinaryIntSolutionVnsSupport;

    using FunctionOneVariableProblemSolver = Teaching.FunctionOneVariableProblem.FunctionOneVariableProblem_solver.FunctionOneVariableProblemSolver;

    using System.Collections.Generic;

    using System;

    public static class Solver
    {

        ///  
        ///     This function executes solver.
        /// 
        ///     Which solver will be executed depends of command-line parameter algorithm.
        ///     
        public static object main()
        {
            object rSeed;
            object OutputControl;
            object outputFile_name;
            object outputFile_ext;
            object outputFile_path_parts;
            object should_add_timestamp_to_file_name;
            object writeToOutputFile;
            object is_minimization;
            try
            {
                logger.debug("Solver started.");
                var parameters = default_parameters_cl;
                var read_parameters_cl = parse_arguments();
                foreach (var param_keyValue in read_parameters_cl._get_kwargs())
                {
                    var key = param_keyValue[0];
                    var val = param_keyValue[1];
                    logger.debug("key:{} value:{}".format(key, val));
                    if (key is not null && val is not null)
                    {
                        parameters[key] = val;
                    }
                }
                logger.debug("Execution parameters: " + parameters.ToString());
                /// set optimization type (minimization or maximization)
                if (parameters["optimization_type"] == "minimization")
                {
                    is_minimization = true;
                }
                else if (parameters["optimization_type"] == "maximization")
                {
                    is_minimization = false;
                }
                else
                {
                    throw new ValueError("Either minimization or maximization should be selected.");
                }
                /// write to output file setup
                if (parameters["writeToOutputFile"] is null)
                {
                    writeToOutputFile = false;
                }
                else
                {
                    writeToOutputFile = @bool(parameters["writeToOutputFile"]);
                }
                /// output file setup
                if (writeToOutputFile)
                {
                    if (parameters["outputFileNameAppendTimeStamp"] is null)
                    {
                        should_add_timestamp_to_file_name = false;
                    }
                    else
                    {
                        should_add_timestamp_to_file_name = @bool(parameters["outputFileNameAppendTimeStamp"]);
                    }
                    if (parameters["outputFilePath"] is not null && parameters["outputFilePath"] != "")
                    {
                        outputFile_path_parts = parameters["outputFilePath"].split("/");
                    }
                    else
                    {
                        outputFile_path_parts = new List<string> {
                            "outputs",
                            "out"
                        };
                    }
                    var outputFile_name_ext = outputFile_path_parts[^1];
                    var outputFile_name_parts = outputFile_name_ext.split(".");
                    if (outputFile_name_parts.Count > 1)
                    {
                        outputFile_ext = outputFile_name_parts[^1];
                        outputFile_name_parts.pop();
                        outputFile_name = ".".join(outputFile_name_parts);
                    }
                    else
                    {
                        outputFile_ext = "txt";
                        outputFile_name = outputFile_name_parts[0];
                    }
                    var dt = datetime.now();
                    outputFile_path_parts.pop();
                    var outputFile_dir = "/".join(outputFile_path_parts);
                    if (should_add_timestamp_to_file_name)
                    {
                        outputFile_path_parts.append(outputFile_name + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "-" + dt.strftime("%Y-%m-%d-%H-%M-%S.%f") + "." + outputFile_ext);
                    }
                    else
                    {
                        outputFile_path_parts.append(outputFile_name + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "." + outputFile_ext);
                    }
                    var outputFile_path = "/".join(outputFile_path_parts);
                    logger.debug("Output file path: " + outputFile_path.ToString());
                    ensure_dir(outputFile_dir);
                    var outputFile = open(outputFile_path, "w", encoding: "utf-8");
                }
                /// output control setup
                if (writeToOutputFile)
                {
                    var output_fields = parameters["outputFields"];
                    var output_moments = parameters["outputMoments"];
                    OutputControl = OutputControl(writeToOutput: true, outputFile: outputFile, fields: output_fields, moments: output_moments);
                }
                else
                {
                    OutputControl = OutputControl(writeToOutput: false);
                }
                /// input file setup
                var input_file_path = parameters["inputFilePath"];
                var input_format = parameters["inputFormat"];
                /// random seed setup
                if (Convert.ToInt32(parameters["randomSeed"]) > 0)
                {
                    rSeed = Convert.ToInt32(parameters["randomSeed"]);
                    logger.info(string.Format("RandomSeed is predefined. Predefined seed value:  %d", rSeed));
                    if (writeToOutputFile)
                    {
                        outputFile.write(string.Format("# RandomSeed is predefined. Predefined seed value:  %d\n", rSeed));
                    }
                    random.seed(rSeed);
                }
                else
                {
                    rSeed = randrange(sys.maxsize);
                    logger.info(string.Format("RandomSeed is not predefined. Generated seed value:  %d", rSeed));
                    if (writeToOutputFile)
                    {
                        outputFile.write(string.Format("# RandomSeed is not predefined. Generated seed value:  %d\n", rSeed));
                    }
                    seed(rSeed);
                }
                /// finishing criteria setup
                var finish_criteria = parameters["finishCriteria"];
                var max_numberEvaluations = parameters["finishEvaluationsMax"];
                var max_numberIterations = parameters["finishIterationsMax"];
                var max_time_for_execution_in_seconds = parameters["finishSecondsMax"];
                var finishControl = FinishControl(criteria: finish_criteria, evaluationsMax: max_numberEvaluations, iterationsMax: max_numberIterations, secondsMax: max_time_for_execution_in_seconds);
                /// solution evaluations and calculations cache setup
                var evaluationCacheIsUsed = parameters["solutionEvaluationCacheIsUsed"];
                var evaluationCacheMaxSize = parameters["solutionEvaluationCacheMaxSize"];
                var calculation_solutionDistanceCacheIsUsed = parameters["solutionDistanceCalculationCacheIsUsed"];
                var calculation_solutionDistanceCacheMaxSize = parameters["solutionDistanceCalculationCacheMaxSize"];
                /// additional statistic control setup
                var additionalStatistics_keep = parameters["additionalStatisticsKeep"];
                var maxLocalOptima = parameters["additionalStatisticsMaxLocalOptima"];
                var additionalStatisticsControl = AdditionalStatisticsControl(keep: additionalStatistics_keep, maxLocalOptima: maxLocalOptima);
                /// problem to be solved
                var problem = FunctionOneVariableProblem.from_input_file(input_file_path: input_file_path, input_format: input_format);
                var start_time = datetime.now();
                if (writeToOutputFile)
                {
                    outputFile.write("# {} started at: {}\n".format(parameters["algorithm"], start_time.ToString()));
                    outputFile.write("# Execution parameters: {}\n".format(parameters));
                }
                /// select among algorithm types
                if (parameters["algorithm"] == "variable_neighborhood_search")
                {
                    /// parameters for VNS process setup
                    var kMin = parameters["kMin"];
                    var kMax = parameters["kMax"];
                    var localSearchType = parameters["localSearchType"];
                    /// initial solution and vns support
                    var solution_type = parameters["solutionType"];
                    object vns_support = null;
                    if (solution_type == "int")
                    {
                        var numberOfIntervals = parameters["solutionNumberOfIntervals"];
                        var solution = FunctionOneVariableProblemBinaryIntSolution(domain_from: problem.domainLow, domain_to: problem.domainHigh, numberOfIntervals: numberOfIntervals, randomSeed: rSeed);
                        vns_support = FunctionOneVariableProblemBinaryIntSolutionVnsSupport();
                    }
                    else
                    {
                        throw new ValueError("Invalid solution/representation type is chosen.");
                    }
                    /// solver construction parameters
                    var vns_construction_params = VnsOptimizerConstructionParameters();
                    vns_construction_params.OutputControl = OutputControl;
                    vns_construction_params.TargetProblem = problem;
                    vns_construction_params.initialSolution = solution;
                    vns_construction_params.problemSolutionVnsSupport = vns_support;
                    vns_construction_params.finishControl = finishControl;
                    vns_construction_params.randomSeed = rSeed;
                    vns_construction_params.additionalStatisticsControl = additionalStatisticsControl;
                    vns_construction_params.kMin = kMin;
                    vns_construction_params.kMax = kMax;
                    vns_construction_params.maxLocalOptima = maxLocalOptima;
                    vns_construction_params.localSearchType = localSearchType;
                    var solver = FunctionOneVariableProblemSolver.from_variable_neighborhood_search(vns_construction_params);
                }
                else
                {
                    throw new ValueError("Invalid optimization algorithm is chosen.");
                }
                solver.Opt.optimize();
                logger.debug("Method -{}- search finished.".format(parameters["algorithm"]));
                logger.info("Best solution code: {}".format(solver.Opt.bestSolution.stringRepresentation()));
                logger.info("Best solution objective: {}, fitness: {}".format(solver.Opt.bestSolution.objectiveValue, solver.Opt.bestSolution.fitnessValue));
                logger.info("Number of iterations: {}, evaluations: {}".format(solver.Opt.iteration, solver.Opt.evaluation));
                logger.info("Execution: {} - {}".format(solver.Opt.executionStarted, solver.Opt.executionEnded));
                logger.debug("Solver ended.");
                return;
            }
            catch (Exception)
            {
                if (hasattr(exp, "message"))
                {
                    logger.exception(string.Format("Exception: %s\n", exp.message));
                }
                else
                {
                    logger.exception(string.Format("Exception: %s\n", exp.ToString()));
                }
            }
        }

        /// This means that if this script is executed, then 
        /// main() will be executed
        static solver()
        {
            if (_name__ == "__main__")
            {
            }
        }
    }
}
