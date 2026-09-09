import requests
import json
import sys

def get_token(username="admin", password="password123", base_url="http://localhost:5190"):
    """
    Fungsi untuk mendapatkan Bearer Token dari API Auth
    """
    url = f"{base_url}/api/Auth/login"
    
    payload = {
        "username": username,
        "password": password
    }
    
    headers = {
        "Content-Type": "application/json"
    }
    
    try:
        response = requests.post(url, json=payload, headers=headers)
        
        if response.status_code == 200:
            data = response.json()
            token = data.get("token")
            print("✅ Login berhasil!")
            print("\n" + "="*60)
            print("BEARER TOKEN:")
            print("="*60)
            print(token)
            print("="*60)
            print("\n📋 Gunakan token ini di Swagger:")
            print(f"   Bearer {token}")
            return token
        else:
            print(f"❌ Login gagal! Status: {response.status_code}")
            print(f"   Response: {response.text}")
            return None
            
    except requests.exceptions.ConnectionError:
        print("❌ Gagal terhubung ke server. Pastikan API sedang berjalan!")
        print("   Jalankan: dotnet run --project src/SkyworxCredit.Api/SkyworxCredit.Api.csproj --launch-profile http")
        return None
    except Exception as e:
        print(f"❌ Error: {e}")
        return None

if __name__ == "__main__":
    # Bisa dijalankan dengan parameter custom
    username = sys.argv[1] if len(sys.argv) > 1 else "admin"
    password = sys.argv[2] if len(sys.argv) > 2 else "password123"
    base_url = sys.argv[3] if len(sys.argv) > 3 else "http://localhost:5190"
    
    get_token(username, password, base_url)