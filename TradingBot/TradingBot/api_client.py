import requests
import json

class ApiClient:
    def __init__(self, base_url, email, password):
        self.base_url = base_url
        self.email = email
        self.password = password
        self.token = None
        self.refresh_token = None

    def login(self):
        response = requests.post(
            f"{self.base_url}/api/auth/login",
            json={"email": self.email, "password": self.password}
        )
        if response.status_code == 200:
            data = response.json()
            self.token = data["accessToken"]
            self.refresh_token = data["refreshToken"]
            return True
        return False

    def get_trades(self):
        headers = {"Authorization": f"Bearer {self.token}"}
        response = requests.get(f"{self.base_url}/api/trades", headers=headers)
        if response.status_code == 200:
            return response.json()
        return None