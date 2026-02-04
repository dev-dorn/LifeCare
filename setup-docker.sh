#!/bin/bash

echo "🏥 LifeCare HMS Docker Setup"
echo "============================"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check Docker and Docker Compose
if ! command -v docker &> /dev/null; then
    echo -e "${RED}❌ Docker is not installed${NC}"
    echo "Please install Docker from: https://docs.docker.com/get-docker/"
    exit 1
fi

if ! command -v docker-compose &> /dev/null; then
    echo -e "${RED}❌ Docker Compose is not installed${NC}"
    echo "Please install Docker Compose from: https://docs.docker.com/compose/install/"
    exit 1
fi

echo -e "${GREEN}✅ Docker and Docker Compose are installed${NC}"

# Generate SSL certificate for development
echo -e "${YELLOW}🔐 Generating development SSL certificate...${NC}"
if [ ! -f "$HOME/.aspnet/https/aspnetapp.pfx" ]; then
    mkdir -p "$HOME/.aspnet/https"
    dotnet dev-certs https -ep "$HOME/.aspnet/https/aspnetapp.pfx" -p password
    dotnet dev-certs https --trust
    echo -e "${GREEN}✅ SSL certificate generated${NC}"
else
    echo -e "${GREEN}✅ SSL certificate already exists${NC}"
fi

# Create Docker volumes if they don't exist
echo -e "${YELLOW}📦 Creating Docker volumes...${NC}"
docker volume create life-care-data 2>/dev/null || true
docker volume create life-care-logs 2>/dev/null || true

echo -e "${GREEN}✅ Setup complete!${NC}"
echo ""
echo -e "${YELLOW}Available commands:${NC}"
echo "  docker-compose up -d          # Start API in production mode"
echo "  docker-compose --profile dev up -d  # Start API in development mode"
echo "  docker-compose --profile test up testbench  # Run tests"
echo "  docker-compose --profile tools up db-viewer  # Start database viewer"
echo "  docker-compose logs -f api    # View API logs"
echo "  docker-compose down           # Stop all services"