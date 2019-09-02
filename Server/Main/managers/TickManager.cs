using Server.Main.objects;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Server.Game.controllers;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Utils;

namespace Server.Main.managers
{
    class TickManager
    {
        private readonly ConcurrentDictionary<int, ITick> _tickers = new ConcurrentDictionary<int, ITick>();

        private readonly ConcurrentDictionary<int, ITick> _serverControllers = new ConcurrentDictionary<int, ITick>();
        
        private readonly ConcurrentDictionary<int, ITick> _playerControllers = new ConcurrentDictionary<int, ITick>();
        
        private int GetNextTickId()
        {
            var i = 0;
            while (true)
            {
                if (_tickers.ContainsKey(i))
                    i++;
                else return i;
            }
        }
        
        private int GetNextPlayerTickId()
        {
            var i = 0;
            while (true)
            {
                if (_playerControllers.ContainsKey(i))
                    i++;
                else return i;
            }
        }

        private int GetNextServerTickId()
        {
            var i = 0;
            while (true)
            {
                if (_serverControllers.ContainsKey(i))
                    i++;
                else return i;
            }
        }

        public void Initialize()
        {
            var tickThread = new Thread(() =>
            {
                Tick(_tickers, 60);
            });
            
            var playerThread = new Thread(() =>
            {
                Tick(_playerControllers, 30);
            });

            var serverThread = new Thread(() =>
            {
                Tick(_serverControllers, 15);
            });

            Task.Factory.StartNew(() =>
            {
                tickThread.Start();
                playerThread.Start();
                serverThread.Start();
            });
        }
        
        public void Add(ITick tick, out int id)
        {
            if (tick == null)
            {
                Out.QuickLog("Trying to add Null tick");
                throw new Exception("Trying to add an invalid class to ticker");
            }
            switch (tick)
            {
                case PlayerController _:
                    id = GetNextPlayerTickId();
                    _playerControllers.TryAdd(id, tick);
                    break;
                case ServerImplementedController _:
                    id = GetNextServerTickId();
                    _serverControllers.TryAdd(id, tick);
                    break;
                default:
                    id = GetNextTickId();
                    _tickers.TryAdd(id, tick);
                    break;
            }
        }

        public void Remove(ITick tick)
        {
            switch (tick)
            {
                case PlayerController _:
                    RemoveFromDictionary(_playerControllers, tick);
                    break;
                case ServerImplementedController _:
                    RemoveFromDictionary(_serverControllers, tick);
                    break;
                default:
                    RemoveFromDictionary(_tickers, tick);
                    break;
            }
        }

        private void RemoveFromDictionary(ConcurrentDictionary<int, ITick> dictionary, ITick tick)
        {
            _tickers.TryRemove(tick.TickId, out _);
        }

        public bool Exists(ITick tickable)
        {
            if (_tickers.Count == 0) return false;
            if (_tickers.ContainsKey(tickable.TickId)) return true;
            return false;
        }

        private void Tick(ConcurrentDictionary<int, ITick> targetTickDictionary, int delayMs)
        {
            while (true)
            {
                ITick current = null;
                try
                {
                    foreach (var tick in targetTickDictionary.Values)
                    {
                        current = tick;
                        tick.Tick();
                    }
                }

                catch (Exception e)
                {
                    if (current != null)
                    {
                        var id = current.TickId;
                        Console.WriteLine("Error at tick " + id);
                        _tickers.TryRemove(id, out _);
                    }

                    Console.WriteLine("Caught exception on Tick");
                    Console.WriteLine(e);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                
                Thread.Sleep(delayMs);
            }
        }
    }
}
