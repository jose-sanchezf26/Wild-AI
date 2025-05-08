import asyncio
import websockets

clients = set()

async def handler(websocket, path):
    clients.add(websocket)
    try:
        async for message in websocket:
            print(f"📩 Recibido: {message}")
            await websocket.send(f"✅ Servidor recibió: {message}")
    except websockets.exceptions.ConnectionClosed:
        print("❌ Cliente desconectado")
    finally:
        clients.remove(websocket)

async def main():
    server = await websockets.serve(handler, "localhost", 8000)
    print("🚀 Servidor WebSocket corriendo en ws://localhost:8000")
    await server.wait_closed()

asyncio.run(main())
