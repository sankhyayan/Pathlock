using TaskManager.API.Application.DTOs;
using TaskManager.API.Application.Interfaces;
using TaskManager.API.Core.Entities;

namespace TaskManager.API.Application.Services
{
    public class SchedulingService : ISchedulingService
    {
        public ScheduleResponse GenerateSchedule(List<TaskItem> tasks)
        {
            if (tasks == null || tasks.Count == 0)
            {
                return new ScheduleResponse();
            }

            // Build dependency graph
            var graph = BuildDependencyGraph(tasks);
            
            // Perform topological sort with constraints
            var orderedTaskIds = TopologicalSort(graph, tasks);
            
            // Map to titles for human-readable output
            var taskMap = tasks.ToDictionary(t => t.Id);
            var orderedTitles = orderedTaskIds
                .Select(id => taskMap.ContainsKey(id) ? taskMap[id].Title : "Unknown")
                .ToList();
            
            return new ScheduleResponse 
            { 
                RecommendedOrder = orderedTaskIds,
                RecommendedTitles = orderedTitles
            };
        }
        
        private Dictionary<Guid, List<Guid>> BuildDependencyGraph(List<TaskItem> tasks)
        {
            var graph = new Dictionary<Guid, List<Guid>>();
            
            foreach (var task in tasks)
            {
                if (!graph.ContainsKey(task.Id))
                {
                    graph[task.Id] = new List<Guid>();
                }
                
                // For each dependency, add an edge: dependency -> task
                foreach (var depId in task.DependsOn ?? new List<Guid>())
                {
                    if (!graph.ContainsKey(depId))
                    {
                        graph[depId] = new List<Guid>();
                    }
                    
                    graph[depId].Add(task.Id); // depId must come before task.Id
                }
            }
            
            return graph;
        }
        
        private List<Guid> TopologicalSort(Dictionary<Guid, List<Guid>> graph, List<TaskItem> tasks)
        {
            var result = new List<Guid>();
            var taskMap = tasks.ToDictionary(t => t.Id);
            
            // Calculate in-degree (number of dependencies) for each task
            var inDegree = new Dictionary<Guid, int>();
            foreach (var task in tasks)
            {
                inDegree[task.Id] = task.DependsOn?.Count ?? 0;
            }
            
            // Use a priority queue that sorts by:
            // 1. In-degree (tasks with no dependencies first)
            // 2. Due date (earliest first)
            // 3. Estimated hours (longest first - critical path)
            var queue = new PriorityQueue<Guid, (int InDegree, long DueDateTicks, double NegativeHours)>();
            
            foreach (var task in tasks)
            {
                if (inDegree[task.Id] == 0)
                {
                    queue.Enqueue(
                        task.Id, 
                        (
                            0, 
                            task.DueDate?.Ticks ?? long.MaxValue,
                            -(task.EstimatedHours ?? 0)
                        )
                    );
                }
            }
            
            while (queue.Count > 0)
            {
                var currentId = queue.Dequeue();
                result.Add(currentId);
                
                // Reduce in-degree for dependent tasks
                if (graph.ContainsKey(currentId))
                {
                    foreach (var dependentId in graph[currentId])
                    {
                        inDegree[dependentId]--;
                        
                        if (inDegree[dependentId] == 0)
                        {
                            var task = taskMap[dependentId];
                            queue.Enqueue(
                                dependentId,
                                (
                                    0,
                                    task.DueDate?.Ticks ?? long.MaxValue,
                                    -(task.EstimatedHours ?? 0)
                                )
                            );
                        }
                    }
                }
            }
            
            // Check for cycles (circular dependencies)
            if (result.Count != tasks.Count)
            {
                throw new InvalidOperationException(
                    "Circular dependency detected in tasks. Cannot generate a valid schedule."
                );
            }
            
            return result;
        }
    }
}
